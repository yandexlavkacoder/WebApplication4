import { useEffect, useState } from 'react'
import { API_URL, clearSession, decodeJwt, loadSession, request, saveSession } from './api'
import './App.css'

const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
const ID_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
const NAME_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'

const STATUS_TEXT = {
  0: 'Нет ответа — браузер заблокировал запрос (CORS) или API не запущен',
  200: 'OK',
  201: 'Created',
  204: 'No Content',
  400: 'Bad Request — некорректные данные',
  401: 'Unauthorized — нет токена, токен поддельный или истёк',
  403: 'Forbidden — токен есть, но прав не хватает',
  404: 'Not Found',
  429: 'Too Many Requests — сработал rate limiting',
  500: 'Ошибка сервера',
}

function statusKind(status) {
  if (status === 0) return 'blocked'
  if (status >= 200 && status < 300) return 'ok'
  if (status === 429) return 'limit'
  return 'denied'
}

export default function App() {
  const [session, setSession] = useState(() => loadSession())
  const [log, setLog] = useState([])
  const [otherId, setOtherId] = useState('1')
  const [busy, setBusy] = useState(false)

  const addLog = (title, method, path, result, note) =>
    setLog((prev) => [{ id: crypto.randomUUID(), title, method, path, note, ...result }, ...prev].slice(0, 40))

  async function call(title, method, path, opts, note) {
    const result = await request(method, path, opts)
    addLog(title, method, path, result, note)
    return result
  }

  function onLoggedIn(data) {
    const user = { userId: data.userId, username: data.username, role: data.role }
    saveSession(data.accessToken, user)
    setSession({ token: data.accessToken, user })
  }

  function logout() {
    clearSession()
    setSession(null)
  }

  async function run(fn) {
    setBusy(true)
    try {
      await fn()
    } finally {
      setBusy(false)
    }
  }

  const token = session?.token
  const me = session?.user

  const checks = [
    {
      key: 'public',
      title: 'Открытый эндпоинт без токена',
      expect: '200 — каталог товаров доступен всем',
      needsLogin: false,
      run: () => call('Каталог (без токена)', 'GET', '/api/products?pageSize=3'),
    },
    {
      key: 'own',
      title: 'Свой профиль с токеном',
      expect: '200 — владелец ресурса',
      needsLogin: true,
      run: () => call('Мой профиль', 'GET', `/api/users/${me.userId}`, { token }),
    },
    {
      key: 'other',
      title: 'Чужой профиль',
      expect: '403 для User, 200 для Admin — resource-based политика',
      needsLogin: true,
      input: true,
      run: () => call(`Профиль пользователя ${otherId}`, 'GET', `/api/users/${otherId}`, { token }),
    },
    {
      key: 'admin',
      title: 'Список всех пользователей',
      expect: '403 для User, 200 для Admin — role-based политика AdminOnly',
      needsLogin: true,
      run: () => call('Все пользователи (AdminOnly)', 'GET', '/api/users?pageSize=5', { token }),
    },
    {
      key: 'manager',
      title: 'Все резервы склада',
      expect: '403 для User, 200 для Admin/Manager — политика CanManageCatalog',
      needsLogin: true,
      run: () => call('Все резервы (Admin/Manager)', 'GET', '/api/stockreservations?pageSize=5', { token }),
    },
    {
      key: 'notoken',
      title: 'Защищённый эндпоинт без токена',
      expect: '401 — сервер не знает, кто ты',
      needsLogin: true,
      run: () => call('Мой профиль БЕЗ токена', 'GET', `/api/users/${me.userId}`),
    },
    {
      key: 'fake',
      title: 'Подделанный токен',
      expect: '401 — подпись не сходится',
      needsLogin: true,
      run: () =>
        call('Мой профиль с испорченным токеном', 'GET', `/api/users/${me.userId}`, {
          token: token.slice(0, -4) + 'AAAA',
        }),
    },
  ]

  async function spamLogin() {
    for (let i = 1; i <= 6; i++) {
      const r = await call(`Неверный вход #${i}`, 'POST', '/api/auth/login', {
        body: { username: me?.username ?? 'nobody', password: 'wrong-password' },
      })
      if (r.status === 429 || r.status === 0) break
    }
  }

  return (
    <div className="page">
      <header className="header">
        <div>
          <h1>Shop API — клиент</h1>
          <p className="muted">
            Фронтенд на <code>{window.location.origin}</code> → API на <code>{API_URL}</code>
          </p>
        </div>
        {session && (
          <button className="ghost" onClick={logout}>
            Выйти
          </button>
        )}
      </header>

      <CorsInfo />

      <div className="grid">
        <section className="card">
          {session ? <SessionInfo session={session} /> : <AuthForm onLoggedIn={onLoggedIn} onResult={addLog} />}
        </section>

        <section className="card">
          <h2>Проверки</h2>
          {!session && <p className="muted">Войдите, чтобы стали доступны запросы с токеном.</p>}
          <ul className="checks">
            {checks.map((c) => (
              <li key={c.key}>
                <div>
                  <strong>{c.title}</strong>
                  <div className="muted small">Ожидается: {c.expect}</div>
                </div>
                <div className="check-actions">
                  {c.input && (
                    <input
                      className="id-input"
                      value={otherId}
                      onChange={(e) => setOtherId(e.target.value.replace(/\D/g, ''))}
                      aria-label="Id пользователя"
                    />
                  )}
                  <button disabled={busy || (c.needsLogin && !session)} onClick={() => run(c.run)}>
                    Выполнить
                  </button>
                </div>
              </li>
            ))}
            <li>
              <div>
                <strong>6 неверных входов подряд</strong>
                <div className="muted small">
                  Ожидается: 401, а с 6-й попытки за минуту — 429 (успешные входы тоже считаются)
                </div>
              </div>
              <div className="check-actions">
                <button disabled={busy} onClick={() => run(spamLogin)}>
                  Выполнить
                </button>
              </div>
            </li>
          </ul>
        </section>
      </div>

      <section className="card">
        <div className="log-head">
          <h2>Журнал запросов</h2>
          {log.length > 0 && (
            <button className="ghost" onClick={() => setLog([])}>
              Очистить
            </button>
          )}
        </div>
        {log.length === 0 && <p className="muted">Пока пусто — выполните любую проверку.</p>}
        <ul className="log">
          {log.map((e) => (
            <LogEntry key={e.id} entry={e} />
          ))}
        </ul>
      </section>
    </div>
  )
}

function AuthForm({ onLoggedIn, onResult }) {
  const [mode, setMode] = useState('login')
  const [form, setForm] = useState({ username: '', email: '', password: '' })
  const [error, setError] = useState('')
  const [pending, setPending] = useState(false)

  const set = (k) => (e) => setForm({ ...form, [k]: e.target.value })

  async function submit(e) {
    e.preventDefault()
    setPending(true)
    setError('')
    const path = mode === 'login' ? '/api/auth/login' : '/api/auth/register'
    const body =
      mode === 'login'
        ? { username: form.username, password: form.password }
        : { username: form.username, email: form.email, password: form.password }
    const r = await request('POST', path, { body })
    onResult(mode === 'login' ? 'Вход' : 'Регистрация', 'POST', path, r)
    setPending(false)

    if (r.ok) onLoggedIn(r.data)
    else if (r.status === 0) setError(STATUS_TEXT[0])
    else if (r.status === 429) setError(`Слишком много попыток. Повторите через ${r.retryAfter ?? 60} с.`)
    else setError(r.data?.error ?? `Ошибка ${r.status}`)
  }

  return (
    <form onSubmit={submit} className="auth">
      <div className="tabs" role="tablist">
        <button type="button" className={mode === 'login' ? 'tab active' : 'tab'} onClick={() => setMode('login')}>
          Вход
        </button>
        <button
          type="button"
          className={mode === 'register' ? 'tab active' : 'tab'}
          onClick={() => setMode('register')}
        >
          Регистрация
        </button>
      </div>

      <label>
        Логин
        <input value={form.username} onChange={set('username')} autoComplete="username" required />
      </label>
      {mode === 'register' && (
        <label>
          Email
          <input type="email" value={form.email} onChange={set('email')} autoComplete="email" required />
        </label>
      )}
      <label>
        Пароль
        <input
          type="password"
          value={form.password}
          onChange={set('password')}
          autoComplete={mode === 'login' ? 'current-password' : 'new-password'}
          required
        />
      </label>

      {error && <div className="error">{error}</div>}

      <button type="submit" disabled={pending}>
        {pending ? 'Отправка…' : mode === 'login' ? 'Войти' : 'Зарегистрироваться'}
      </button>
      {mode === 'register' && <p className="muted small">Новый пользователь получает роль User.</p>}
    </form>
  )
}

function SessionInfo({ session }) {
  const claims = decodeJwt(session.token) ?? {}
  const [now, setNow] = useState(() => Date.now())

  useEffect(() => {
    const t = setInterval(() => setNow(Date.now()), 1000)
    return () => clearInterval(t)
  }, [])

  const secondsLeft = claims.exp ? Math.max(0, Math.round(claims.exp - now / 1000)) : null
  const expired = secondsLeft === 0

  return (
    <div>
      <h2>Вы вошли</h2>
      <dl className="facts">
        <dt>Пользователь</dt>
        <dd>{claims[NAME_CLAIM] ?? session.user.username}</dd>
        <dt>Id (claim nameidentifier)</dt>
        <dd>{claims[ID_CLAIM]}</dd>
        <dt>Роль (claim role)</dt>
        <dd>
          <span className="role">{claims[ROLE_CLAIM]}</span>
        </dd>
        <dt>Издатель / аудитория</dt>
        <dd>
          {claims.iss} / {claims.aud}
        </dd>
        <dt>Токен истекает</dt>
        <dd className={expired ? 'error-text' : ''}>
          {expired
            ? 'истёк — запросы получат 401, войдите заново'
            : `через ${Math.floor(secondsLeft / 60)} мин ${secondsLeft % 60} с`}
        </dd>
      </dl>
      <details>
        <summary>Токен (сохранён в localStorage)</summary>
        <code className="token">{session.token}</code>
      </details>
    </div>
  )
}

function CorsInfo() {
  const allowed = window.location.origin === 'http://localhost:5173'
  if (allowed) {
    return (
      <p className="muted small">
        Отказ CORS: <a href="http://localhost:5174">http://localhost:5174</a>
      </p>
    )
  }
  return (
    <p className="muted small">
      Эта страница открыта с <code>{window.location.origin}</code>, а API разрешает только{' '}
      <code>http://localhost:5173</code> — браузер заблокирует ответы по CORS. Подробности — в консоли браузера (F12).
    </p>
  )
}

function LogEntry({ entry }) {
  const kind = statusKind(entry.status)
  const body =
    entry.networkError ??
    (typeof entry.data === 'string' ? entry.data : JSON.stringify(entry.data, null, 2)) ??
    ''
  return (
    <li className={`entry ${kind}`}>
      <div className="entry-head">
        <span className={`badge ${kind}`}>{entry.status === 0 ? 'BLOCKED' : entry.status}</span>
        <strong>{entry.title}</strong>
        <code className="muted small">
          {entry.method} {entry.path}
        </code>
        <span className="muted small push">{entry.ms} мс</span>
      </div>
      <div className="small">{STATUS_TEXT[entry.status] ?? ''}</div>
      {entry.retryAfter && <div className="small">Retry-After: {entry.retryAfter} с</div>}
      {body && (
        <details>
          <summary className="small">Тело ответа</summary>
          <pre>{body}</pre>
        </details>
      )}
    </li>
  )
}
