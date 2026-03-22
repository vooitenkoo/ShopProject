# Shop

Микросервисный бэкенд интернет-магазина на **.NET 9**: регистрация и аутентификация пользователей, каталог товаров и тегов, единая точка входа через API-шлюз.

## Архитектура

| Компонент | Назначение |
|-----------|------------|
| **UserManagmentService** | Пользователи: регистрация, вход (JWT), CRUD по профилю |
| **ProductManagmentService** | Товары и теги, связи товар–тег, политики ролей (Admin / Client) |
| **ApiGateway** | Маршрутизация запросов (**Ocelot**) к сервисам |
| **PostgreSQL** | Общее хранилище данных для сервисов |

Сервисы построены по слоям: **Domain**, **Application** (MediatR, валидация), **Infrastructure**, **Web API**.

## Стек

- ASP.NET Core 9, Entity Framework Core, Npgsql  
- JWT Bearer, MediatR, FluentValidation, AutoMapper (где используется)  
- Ocelot (шлюз), Docker / Docker Compose  
- xUnit — тесты в `UserManagmentService/test`

## Требования

- [.NET SDK 9](https://dotnet.microsoft.com/download) — для локального запуска и тестов  
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — для контейнеров

## Запуск через Docker

Из корня репозитория (`Shop`):

```bash
docker compose up --build
```

После старта:

- **API Gateway:** `http://localhost:3000`  
- **User service:** `http://localhost:3001`  
- **Product service:** `http://localhost:3002`  
- **PostgreSQL:** `localhost:5432` (пользователь/пароль по умолчанию: `postgres` / `postgres`, БД `db`)

> **Замечание:** в `ApiGateway/ocelot.json` для downstream указаны `localhost` и порты сервисов. Для сценария «только Docker» может потребоваться замена хостов на имена сервисов из `docker-compose` (`user-service`, `product-service`) и схемы `http`, если шлюз не достучится до бэкендов.

## Локальная разработка

1. Поднять PostgreSQL (или использовать контейнер только с БД).  
2. Заполнить конфигурацию (в проектах используются `appsettings` и при необходимости `appsettings.env.json` — строка подключения к БД и секция **Jwt**).  
3. Применить миграции EF Core для нужного сервиса.  
4. Запуск отдельных API:

```bash
cd UserManagmentService/src/WebApi
dotnet run
```

```bash
cd ProductManagmentService/src/WebApi
dotnet run
```

```bash
cd ApiGateway
dotnet run
```

Swagger у сервисов обычно доступен в режиме разработки (см. `launchSettings.json` и `Program.cs`).

## Тесты

```bash
cd UserManagmentService/test
dotnet test
```

```bash
cd ProductManagmentService/test/ProductManagmentServiceTests
dotnet test
```

## Структура репозитория

```
Shop/
├── ApiGateway/              # Ocelot, единая точка входа
├── UserManagmentService/    # Сервис пользователей
├── ProductManagmentService/ # Сервис товаров и тегов
└── docker-compose.yml
```

## Лицензия

Укажите лицензию при публикации проекта.
