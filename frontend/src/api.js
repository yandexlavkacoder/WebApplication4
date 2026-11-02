export const API_URL = import.meta.env.VITE_API_URL ?? 'https://localhost:7218'

const TOKEN_KEY = 'shop.accessToken'
const USER_KEY = 'shop.user'

export function loadSession() {
  try {
    const token = localStorage.getItem(TOKEN_KEY)
    const user = JSON.parse(localStorage.getItem(USER_KEY) ?? 'null')
    return token && user ? { token, user } : null
  } catch {
    return null
  }
}

export function saveSession(token, user) {
  try {
    localStorage.setItem(TOKEN_KEY, token)
    localStorage.setItem(USER_KEY, JSON.stringify(user))
  } catch {
    return
  }
}

export function clearSession() {
  try {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
  } catch {
    return
  }
}

export function decodeJwt(token) {
  try {
    const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    const json = decodeURIComponent(
      atob(payload)
        .split('')
        .map((c) => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
        .join(''),
    )
    return JSON.parse(json)
  } catch {
    return null
  }
}

export async function request(method, path, { token, body } = {}) {
  const headers = {}
  if (body !== undefined) headers['Content-Type'] = 'application/json'
  if (token) headers['Authorization'] = `Bearer ${token}`

  const started = performance.now()
  try {
    const res = await fetch(API_URL + path, {
      method,
      headers,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    })
    const text = await res.text()
    let data
    try {
      data = text ? JSON.parse(text) : null
    } catch {
      data = text
    }
    return {
      ok: res.ok,
      status: res.status,
      data,
      retryAfter: res.headers.get('Retry-After'),
      ms: Math.round(performance.now() - started),
    }
  } catch (err) {
    return {
      ok: false,
      status: 0,
      data: null,
      networkError: err.message,
      ms: Math.round(performance.now() - started),
    }
  }
}
