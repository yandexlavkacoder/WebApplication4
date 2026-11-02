# Shop API — клиент (React + Vite)

Отдельный фронтенд для проверки JWT-аутентификации, авторизации, CORS и rate limiting
API из папки `WebApplication4`. Работает на своём origin `http://localhost:5173`
и ходит в API на `https://localhost:7218` (адрес — в `.env`, переменная `VITE_API_URL`).

## Запуск

1. Запустить API (профиль **https**) из Visual Studio или командой:

   ```bash
   dotnet run --project WebApplication4 --launch-profile https
   ```

2. В папке `frontend` (первый раз — `npm install`):

   ```bash
   npm run dev
   ```

3. Открыть http://localhost:5173

## Тестовые учётные записи

| Логин | Пароль | Роль |
|---|---|---|
| `test_user1` | `TestUser123` | User |
| `test_admin1` | `TestAdmin123` | Admin |

Новые пользователи из формы «Регистрация» получают роль `User`.

## Что показывает экран

| Проверка | User | Admin |
|---|---|---|
| Каталог без токена | 200 | 200 |
| Свой профиль с токеном | 200 | 200 |
| Чужой профиль (resource-based, `OwnerOrAdmin`) | **403** | 200 |
| Все пользователи (role-based, `AdminOnly`) | **403** | 200 |
| Все резервы (role-based, `CanManageCatalog`) | **403** | 200 |
| Защищённый эндпоинт без токена | **401** | **401** |
| Подделанный токен | **401** | **401** |
| 6 неверных входов подряд | 401 … **429** | 401 … **429** |

## Как показать отказ CORS

```bash
npm run dev:cors-fail
```

Откроется тот же фронтенд, но на `http://localhost:5174` — этого origin нет в
`Cors:AllowedOrigins`. Любой запрос получит в журнале статус **BLOCKED**, а в консоли
браузера (F12) — «blocked by CORS policy». Сервер при этом запрос выполнил: блокирует
именно браузер, не отдавая ответ странице с чужого origin. Поэтому CORS и нельзя проверить
через Swagger или Postman — они не браузерные страницы с другим origin.
