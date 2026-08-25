# Task Manager API

REST API developed in ASP.NET Core for managing tasks, notes, and users, utilizing JWT authentication and a layered architecture.

## Technologies

- ASP.NET Core
- C#
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

https://localhost:5129/swagger

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Auth/register` | Register a new user |
| POST | `/api/Auth/login` | Authenticate a user and generate a JWT token |

---

### Users

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Users` | Get the authenticated user's profile |
| PUT | `/api/Users/me` | Update the authenticated user's information |
| DELETE | `/api/Users/me` |  Delete the authenticated user's account |
| PUT | `/api/Users/password` | Change the authenticated user's password |

---

### Tasks

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Taskitem` | Get all tasks from the authenticated user |
| POST | `/api/Taskitem` | Create a new task |
| PUT | `/api/Taskitem/{id}` | Update an existing task |
| PATCH | `/api/Taskitem/{id}/complete` | Mark a task as completed |
| DELETE | `/api/Taskitem/{id}` | Delete a task |

---

### Task Notes

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Tasknote/{taskItemId}` | Get all notes from a task |
| POST | `/api/Tasknote/{taskItemId}` | Create a new note for a task |
| PUT | `/api/Tasknote/{id}` | Update a task note |
| DELETE | `/api/Tasknote/{id}` | Delete a task note |

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
