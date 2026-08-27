# Task Manager API

REST API developed with ASP.NET Core for managing tasks, notes, and users, using JWT authentication, Entity Framework Core, MySQL, and Clean Architecture.

## Technologies

* C#
* ASP.NET Core 8
* Entity Framework Core
* MySQL 8
* JWT Authentication
* API Versioning
* Docker & Docker Compose
* Swagger / OpenAPI
* AutoMapper

## Features

* User registration
* JWT-based authentication
* User profile management
* Password change
* Account deletion
* Task CRUD
* Task completion
* Task notes CRUD
* Task priority
* Task due dates
* User-task association
* Protected endpoints using JWT authentication

## Architecture

The project follows the principles of Clean Architecture, separating responsibilities into the following layers:

```text
src/
├── Domain/
├── Application/
├── Infrastructure/
└── Presentation/
    └── WebAPI/
```

### Layers

* **Domain** — Entities, enums, and domain exceptions.
* **Application** — Application services, DTOs, and interfaces.
* **Infrastructure** — Database context, repositories, and infrastructure services.
* **Presentation** — ASP.NET Core Web API, controllers, and application configuration.

## Project Structure

```text
TaskManagerAPI/
├── src/
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   └── Presentation/
│       └── WebAPI/
│
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── .gitignore
├── TaskManager.slnx
└── README.md
```

## Requirements

To run the project using Docker, you only need:

* Docker
* Docker Compose

The application uses:

* **MySQL 8** as the database
* **ASP.NET Core 8** for the Web API

## Configuration

The application configuration is provided through environment variables when running with Docker Compose.

### Database

The API connects to MySQL using the Docker Compose service name:

```text
Server=mysql
Port=3306
Database=TaskManager
User=root
Password=${MYSQL_ROOT_PASSWORD}
```

The MySQL container is exposed to the host on port `3307`, while containers communicate internally through port `3306`.

### JWT

The JWT signing key is provided through:

```env
JWTKEY=your_secure_jwt_secret_key
```

The Docker Compose configuration maps this value to:

```text
Jwt__Key
```

## Database Migrations

> Database migrations are applied automatically when the API starts using Entity Framework Core's `Database.Migrate()` method.

## Swagger

Swagger / OpenAPI is available when the application is running:

```text
http://localhost:5129/swagger
```

Swagger can be used to explore and test the API endpoints.

## How to run with Docker

### 1. Clone the repository

```bash
git clone https://github.com/Rafael-fM24/TaskManagerAPI.git
cd TaskManagerAPI
```

### 2. Configure environment variables

Create a `.env` file in the project root:

```env
MYSQL_ROOT_PASSWORD=your_secure_mysql_password
JWTKEY=your_secure_jwt_secret_key
```

Do not commit the `.env` file to the repository.

### 3. Start the containers

```bash
docker compose up --build
```

Docker Compose will start:

* MySQL
* Task Manager Web API

The API waits for MySQL to become healthy before starting.

### 4. Stop the application

```bash
docker compose down
```

To remove the MySQL volume and its persisted data:

```bash
docker compose down -v
```

> **Warning:** `docker compose down -v` permanently removes the MySQL data stored in the Docker volume.

## How to run without Docker

### 1. Configure the database

Configure the connection string in `appsettings.json` or through environment variables.

### 2. Start MySQL

Make sure a MySQL 8 instance is running and accessible using the configured connection string.

### 3. Run the application

```bash
dotnet restore
dotnet run --project src/Presentation/WebAPI/WebAPI.csproj
```

## API Versioning

The API uses URL-based versioning to support future changes without
breaking existing clients.

Current version:

- `v1`

## API Endpoints

### Authentication

| Method | Endpoint                | Description                                  |
| ------ |-------------------------| -------------------------------------------- |
| POST   | `/api/v1/Auth/register` | Register a new user                          |
| POST   | `/api/v1/Auth/login`    | Authenticate a user and generate a JWT token |

---

### Users

| Method | Endpoint                 | Description                                 |
| ------ |--------------------------| ------------------------------------------- |
| GET    | `/api/v1/Users/me`       | Get the authenticated user's profile        |
| PUT    | `/api/v1/Users/me`       | Update the authenticated user's information |
| DELETE | `/api/v1/Users/me`       | Delete the authenticated user's account     |
| PUT    | `/api/v1/Users/password` | Change the authenticated user's password    |

---

### Tasks

| Method | Endpoint                         | Description                               |
| ------ |----------------------------------| ----------------------------------------- |
| GET    | `/api/v1/Taskitem`               | Get all tasks from the authenticated user |
| POST   | `/api/v1/Taskitem`               | Create a new task                         |
| PUT    | `/api/v1/Taskitem/{id}`          | Update an existing task                   |
| PATCH  | `/api/v1/Taskitem/{id}/complete` | Mark a task as completed                  |
| DELETE | `/api/v1/Taskitem/{id}`          | Delete a task                             |

---

### Task Notes

| Method | Endpoint                        | Description                  |
| ------ |---------------------------------| ---------------------------- |
| GET    | `/api/v1/Tasknote/{taskItemId}` | Get all notes from a task    |
| POST   | `/api/v1/Tasknote/{taskItemId}` | Create a new note for a task |
| PUT    | `/api/v1/Tasknote/{id}`         | Update a task note           |
| DELETE | `/api/v1/Tasknote/{id}`         | Delete a task note           |

> Protected endpoints require a valid JWT token in the `Authorization` header.

## Authentication

The API uses JWT Bearer authentication.

After logging in, copy the generated token and send it in the `Authorization` header:

```http
Authorization: Bearer {token}
```

In Swagger, click **Authorize** and enter:

```text
Bearer {token}
```

## Docker Services

The Docker Compose configuration contains two services:

| Service  | Description          | Host Port |
| -------- | -------------------- | --------- |
| `webapi` | ASP.NET Core Web API | `5129`    |
| `mysql`  | MySQL database       | `3307`    |

The services communicate through the Docker Compose network.

The API connects to MySQL using:

```text
mysql:3306
```

rather than `localhost:3307`.

## Future Improvements

* Refresh tokens
* Pagination
* Filtering and sorting
* Structured logging
