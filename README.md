# Vogu Map API

REST API для управления интерактивной картой планов корпусов.

## Требования

### Системные требования

| Компонент | Версия |
|-----------|--------|
| .NET SDK | 10.0 или выше |
| Docker | 20.10 или выше |
| Docker Compose | 2.0 или выше |
| PostgreSQL | 18 (используется в Docker) |

### Установка зависимостей

**На Windows/macOS/Linux:**
- Установите [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Установите [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Стек технологий

- **.NET 10** / ASP.NET Core 10 - веб-фреймворк
- **Docker** + Docker Compose - контейнеризация и оркестрация
- **PostgreSQL 18** + Entity Framework Core 10 (Npgsql) - база данных и ORM
- **Swagger / OpenAPI** - документация API
- **AutoMapper 16** - маппинг между DTOs и сущностями
- **xUnit 2.9** - фреймворк для тестирования

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

2. Проверка доступности сервисов:

| Сервис   | Адрес                              |
|----------|-----------------------------------|
| API      | http://localhost:8080             |
| Swagger UI | http://localhost:8080/swagger    |
| PostgreSQL | localhost:5432 (контейнер)      |

> База данных `postgres` поднимается автоматически в Docker-контейнере.  
> В режиме **Development** миграции применяются самим приложением при старте.

### Локально

1. Установи [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) и [PostgreSQL 18](https://www.postgresql.org/download/).

2. В корне проекта создай файл `.env` на основе `.env.example`:

```bash
cp .env.example .env
```

Отредактируй `.env` с правильными параметрами:

```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=vogumap
DB_USER=postgres
DB_PASSWORD=your_password
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://0.0.0.0:5000
```

3. Примени миграции базы данных:

```bash
dotnet ef database update --project src/VoguMap.Infrastructure
```

4. Запусти API:

```bash
dotnet run --project src/VoguMap.Web
```

API будет доступен на `http://localhost:5000`, Swagger на `http://localhost:5000/swagger`.

---

## Переменные окружения

Файл `.env` в корне проекта содержит переменные окружения для API и БД.

| Переменная                | Описание                                              | Пример |
|---------------------------|-------------------------------------------------------|--------|
| `DB_HOST`                 | Host базы данных (в Docker `postgres`, локально `localhost`) | `postgres` |
| `DB_PORT`                 | Порт базы данных | `5432` |
| `DB_NAME`                 | Имя базы данных | `vogumap` |
| `DB_USER`                 | Пользователь PostgreSQL | `postgres` |
| `DB_PASSWORD`             | Пароль PostgreSQL | `password123` |
| `ASPNETCORE_ENVIRONMENT`  | Окружение ASP.NET Core (`Development`/`Production`) | `Production` |
| `ASPNETCORE_URLS`         | URL, на котором слушает Kestrel | `http://0.0.0.0:80` |

---

## Тестирование

### Запуск всех тестов

```bash
dotnet test
```

### Запуск тестов с подробным выводом

```bash
dotnet test --verbosity normal
```

### Запуск специфичного набора тестов

```bash
dotnet test --filter "Category=Integration"
```

### Структура тестов

```
src/VoguMap.Tests/
├── IntegrationTests/
│   ├── BuildingServiceIntegrationTests.cs    # Тесты сервиса корпусов
│   └── RoomServiceIntegrationTests.cs        # Тесты сервиса помещений
├── Repositories/
│   └── *RepositoryTests.cs                   # Юнит-тесты репозиториев
├── Context/
│   └── InMemoryDbContext.cs                  # InMemory контекст для тестов
└── Factories/
    └── *Factory.cs                           # Фабрики для создания тестовых данных
```

### Примеры тестовых команд

**Запуск интеграционных тестов:**
```bash
dotnet test src/VoguMap.Tests/VoguMap.Tests.csproj --filter "FullyQualifiedName~IntegrationTests"
```

**Запуск конкретного теста:**
```bash
dotnet test src/VoguMap.Tests/VoguMap.Tests.csproj --filter "FullyQualifiedName~BuildingServiceIntegrationTests"
```

---

## Доменная модель

### Building (Учебный корпус)

| Поле | Тип | Описание |
|------|-----|---------|
| `Id` | int | Первичный ключ |
| `Name` | string | Название корпуса |
| `Address` | string | Адрес корпуса |
| `Rooms` | List<Room> | Помещения в корпусе (навигационное свойство) |
| `CreatedAt` | DateTime | Дата создания |
| `UpdatedAt` | DateTime? | Дата последнего обновления |

### Room (Помещение)

| Поле | Тип | Описание |
|------|-----|---------|
| `Id` | int | Первичный ключ |
| `Name` | string | Название помещения |
| `Floor` | int | Номер этажа |
| `Capacity` | int | Вместимость помещения |
| `BuildingId` | int | Внешний ключ на Building |
| `Building` | Building | Навигационное свойство на корпус |
| `CreatedAt` | DateTime | Дата создания |
| `UpdatedAt` | DateTime? | Дата последнего обновления |

---

## Мониторинг и логирование

### Встроенное логирование

Приложение использует встроенную систему логирования ASP.NET Core. Логи выводятся в консоль.

### Просмотр логов контейнера

```bash
docker compose logs webapi
```

**Просмотр логов в реальном времени:**
```bash
docker compose logs -f webapi
```

**Просмотр логов только PostgreSQL:**
```bash
docker compose logs postgres
```

---

## Развертывание

### Production окружение

1. **Подготовка переменных окружения:**

Создай `.env` файл с Production переменными:

```env
DB_HOST=your-production-db-host
DB_PORT=5432
DB_NAME=vogumap_prod
DB_USER=prod_user
DB_PASSWORD=strong_password_here
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:80
```

2. **Сборка и запуск в Docker:**

```bash
docker compose -f docker-compose.yml up -d
```

3. **Проверка статуса контейнеров:**

```bash
docker compose ps
```

### Backup базы данных

**Создание бэкапа:**
```bash
docker exec vogumap-postgres pg_dump -U $DB_USER $DB_NAME > backup.sql
```

**Восстановление из бэкапа:**
```bash
docker exec -i vogumap-postgres psql -U $DB_USER $DB_NAME < backup.sql
```

---

## Структура проекта (подробно)

```
src/
├── VoguMap.Web/
│   ├── Controllers/v1/         # REST-контроллеры (Building, Room)
│   ├── Middlewares/            # Промежуточные слои (ExceptionHandler)
│   ├── appsettings.json        # Конфигурация приложения
│   └── Program.cs              # Точка входа, конфигурация DI
│
├── VoguMap.Application/
│   ├── Common/                 # Базовые классы (PagedResultDto, BaseService)
│   ├── DTOs/                   # Data Transfer Objects (BuildingDto, RoomDto)
│   ├── Helpers/                # Вспомогательные классы
│   ├── Mappings/               # AutoMapper-профили (BuildingProfile, RoomProfile)
│   └── Services/               # Интерфейсы и реализации сервисов
│       ├── Interfaces/         # IBuildingService, IRoomService
│       └── Implementations/    # BuildingService, RoomService
│
├── VoguMap.Domain/
│   ├── Entities/               # EF Core-сущности (Building, Room)
│   ├── Interfaces/             # Интерфейсы (IRepository, IUnitOfWork)
│   ├── Filters/                # Классы для фильтрации (RoomFilterDto)
│   └── Exceptions/             # Пользовательские исключения
│
├── VoguMap.Infrastructure/
│   └── Persistence/
│       ├── Context/            # VoguMapContext (DbContext)
│       ├── Repositories/       # Реализации репозиториев (GenericRepository, BuildingRepository)
│       ├── Migrations/         # EF Core-миграции
│       ├── Seed/               # Заполнение справочных данных (ApplicationDbSeeder)
│       └── Factories/          # Фабрика для создания контекста
│
└── VoguMap.Tests/
    ├── Context/                # Имитации DbContext (InMemory)
    ├── Factories/              # Фабрики создания тестовых сущностей
    ├── IntegrationTests/       # Интеграционные тесты сервисов
    │   ├── BuildingServiceIntegrationTests.cs
    │   └── RoomServiceIntegrationTests.cs
    └── Repositories/           # Юнит-тесты репозиториев
        ├── BuildingRepositoryTests.cs
        └── RoomRepositoryTests.cs
```
