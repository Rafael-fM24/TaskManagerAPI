# Task Manager API

REST API developed in ASP.NET Core for managing tasks, notes, and users, utilizing JWT authentication and a layered architecture.

## Technologies

- ASP.NET Core
- W#
- Entity Framework Core
- SQLServer
- JWT Authentication
- Swagger
- AutoMapper

## Features

- User registration
- JWT-based login
- Task CRUD
- Task note CRUD
- Priority setting
- Due date
- User-task association

## Architecture

The project follows a layered architecture:

- API
- Application
- Domain
- Infrastructure

## How to run

Clone the repository

git clone https://github.com/usuario/TaskManager.git

Enter the folder

cd TaskManager

Restore the packages

dotnet restore

Run the migrations

dotnet ef database update

Run the application

dotnet run

## Configuration

Before running the application, configure the required environment settings.

The following values need to be provided:

- Database connection string
- JWT secret key

Example:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  },
  "Jwt": {
    "Key": "your_secret_key"
  }
}
```
## Swagger

After starting the application:

https://localhost:5001/swagger

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate a user and generate a JWT token |

---

### Users

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get the authenticated user's profile |
| PUT | `/api/users` | Update the authenticated user's information |
| PUT | `/api/users/password` | Change the authenticated user's password |

---

### Tasks

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/taskitem` | Get all tasks from the authenticated user |
| POST | `/api/taskitem` | Create a new task |
| PUT | `/api/taskitem/{id}` | Update an existing task |
| PATCH | `/api/taskitem/{id}/complete` | Mark a task as completed |
| DELETE | `/api/taskitem/{id}` | Delete a task |

---

### Task Notes

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasknote/{taskItemId}` | Get all notes from a task |
| POST | `/api/tasknote/{taskItemId}` | Create a new note for a task |
| PUT | `/api/tasknote/{id}` | Update a task note |
| DELETE | `/api/tasknote/{id}` | Delete a task note |

> Protected endpoints require a valid JWT token in the `Authorization` header.

## Authentication

The API uses JWT authentication.

After logging in, use the token in the header:

Authorization: Bearer {token}

## Future improvements

- Docker
- Unit tests
- Refresh token
- Pagination
- Filters
- Logs
