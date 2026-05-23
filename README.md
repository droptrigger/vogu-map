# Vogu Map API

REST API для управления интерактивной картой планов корпусов.

## Стек технологий

- **.NET 10** / ASP.NET Core 10
- **PostgreSQL 18** + Entity Framework Core 10 (Npgsql)
- **AutoMapper 16**
- **Swagger / OpenAPI**
- **Docker** + Docker Compose

## Архитектура

Проект построен по принципам **Clean Architecture** и разбит на 4 слоя:

```
src/
├── VoguMap.Web              # Точка входа: контроллеры, middleware, DI
├── VoguMap.Application      # Бизнес-логика: сервисы, DTO, AutoMapper-профили
├── VoguMap.Domain           # Ядро: сущности, интерфейсы, исключения
├── VoguMap.Infrastructure   # Инфраструктура: EF Core, репозитории, контекст БД
└── VoguMap.Tests            # Тесты: интеграционные и юнит-тесты
```

## Быстрый старт

### Docker (рекомендуется)

1. Запуск проекта в Docker:

```bash
docker compose up --build
```

| Сервис   | Адрес                         |
|----------|-------------------------------|
| API      | http://localhost:8080         |
| Swagger  | http://localhost:8080/swagger |

> База данных `postgres` поднимается автоматически в Docker‑контейнере.  
> В режиме **Development** миграции применяются самим приложением при старте.  
> Для администрирования можно при необходимости добавить сервис `pgadmin` в `docker-compose.yml`.

2. Применение миграций

```bash
dotnet ef database update --project src/VoguMap.Infrastructure
```

> Убедитесь, что `DB_HOST` в `.env` указан правильно (`localhost` для локальной БД или `postgres` при работе с Docker).  
> В `Development` миграции применяются автоматически при запуске приложения.

### Локально

1. Установи [.NET 10 SDK](https://dotnet.microsoft.com/download) и PostgreSQL 18.
2. В корне проекта создай файл `.env` на основе `.env.example` и укажи свои значения.
3. Выполни миграции из корня решения:

```bash
dotnet ef database update --project src/VoguMap.Infrastructure
```

> Для локального подключения к базе в `.env` укажи `DB_HOST=localhost`.  
> В `Development` миграции применяются автоматически при запуске приложения.

4. Запусти API:

```bash
dotnet run --project src/VoguMap.Web
```

---

## Переменные окружения

Файл `.env` в корне проекта (на основе `.env.example`) содержит переменные окружения для API и БД.

| Переменная                | Описание                                              |
|---------------------------|-------------------------------------------------------|
| `DB_HOST`                 | Host базы данных (в Docker обычно `postgres`)        |
| `DB_PORT`                 | Порт базы данных (обычно `5432`)                    |
| `DB_NAME`                 | Имя базы данных                                       |
| `DB_USER`                 | Пользователь PostgreSQL                               |
| `DB_PASSWORD`             | Пароль PostgreSQL                                     |
| `ASPNETCORE_ENVIRONMENT`  | Окружение ASP.NET Core (`Development`/`Production`)  |
| `ASPNETCORE_URLS`         | URL, на котором слушает Kestrel (например, `http://0.0.0.0:80`) |

Пример `.env.example`:

```env
DB_HOST=your-database-host
DB_PORT=your-database-port
DB_NAME=your-database-name
DB_USER=your-database-user
DB_PASSWORD=your-database-password
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:80
```

---

## Доменная модель

| Сущность   | Описание        |
|------------|-----------------|
| `Building` | Учебные корпуса |
| `Room`     | Помещения       |

## Структура проекта (подробно)

```
src/
├── VoguMap.Web/
│   ├── Controllers/v1/         # REST-контроллеры
│   ├── Middlewares/            # Промежуточные слои
│   └── Program.cs              # Точка входа, конфигурация DI
│
├── VoguMap.Application/
│   ├── Common/                 # Базовые классы сервисов и DTO‑помощники
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Helpers/                # Вспомогательные классы
│   ├── Mappings/               # AutoMapper-профили
│   └── Services/               # Интерфейсы и реализации сервисов
│
├── VoguMap.Domain/
│   ├── Entities/               # EF Core-сущности
│   ├── Interfaces/             # Интерфейсы репозиториев
│   ├── Filters/                # Классы для фильтрации получаемых данных
│   └── Exceptions/             # Исключения
│
├── VoguMap.Infrastructure/
│   └── Persistence/
│       ├── Context/            # VoguMapContext (DbContext)
│       ├── Factories/          # VoguMapContext фабрика для применения миграций
│       ├── Repositories/       # Реализации репозиториев
│       ├── Migrations/         # EF Core-миграции
│       └── Seed/               # Заполнение справочных данных
│
└── VoguMap.Tests/
    ├── Context/                # Имитации DbContext (InMemory)
    ├── Factories/              # Фабрики создания тестовых сущностей
    ├── IntegrationTests/       # Интеграционные тесты сервисов
    └── Repositories/           # Юнит-тесты репозиториев
```
