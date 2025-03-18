# TaskManager

## Overview

TaskManager is a backend application designed to manage tasks efficiently. It provides APIs for creating, updating, deleting, and retrieving tasks. The application is built using C# and .NET Core with Entity Framework Core.

## Features

- User authentication and authorization
- Email verification
- Secure password hashing
- Create, update, delete, and retrieve tasks
- Task categorization and prioritization
- Due dates and reminders
- Search and filter tasks
- Lazy loading support for efficient data access
- Pagination for efficient data access
- Standardized response format using BaseResult
- Custom validation error response matching BaseResult format
- Fixed API Key authentication
- Custom exception handling middleware
- Rate limiting middleware

## Technologies & Packages Used

- **.NET Core** - Backend framework
- **Entity Framework Core** - ORM for database interactions
- **MailKit** - Email sending and verification
- **BCrypt.Net** - Secure password hashing
- **AutoMapper** - Object mapping
- **Lazy Loading** - Optimized data fetching
- **Middleware Components:**
  - Exception Handling
  - Rate Limiting
  - Fixed API Key Authentication

## Layered Architecture

- **Data Layer:** Contains entities, DbContext, and migrations.
- **Repository Layer:** Implements the Repository Pattern for data access.
- **Service Layer:** Contains business logic and communicates with the repository.
- **Web Layer:** Exposes APIs, handles configurations, and manages Dependency Injection (DI).

## Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/Abdelrahman548/task-manager-backend.git
   ```

2. Navigate to the project directory:

   ```sh
   cd task-manager-backend
   ```

3. Restore dependencies:

   ```sh
   dotnet restore
   ```

## Configuration

1. Create an `appsettings.json` file in the root directory of the project.
2. Add the following configuration:

   ```json
   {
     "ConnectionStrings": {
       "TaskManagerDB": "Server=your_server;Database=TaskManagerDb;User Id=your_user;Password=your_password;"
     },
     "Jwt": {
        "Issuer": "",
        "Audience": "",
        "LifeTime": 0,
        "SigningKey": "32 char secret key"
     },
     "Email": {
        "SmtpServer": "smtp.example.com",
        "SenderEmail": "your-email@example.com",
        "SenderName": "",
        "PortSSL": 0,
        "Port": 0,
        "AppPassword": "your_password"
     },
     "APIKEY": "your_fixed_api_key"
   }
   ```

## Usage

1. Apply database migrations:

   ```sh
   dotnet ef database update
   ```

2. Run the application:

   ```sh
   dotnet run --launch-profile "https"
   ```

3. The server will be running on `http://localhost:7176`.

## Some API Endpoints

### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Log in a user
- `POST /api/auth/verify-email` - Verify email address
- `POST /api/auth/logout` - Log out a user
- `POST /api/auth/refresh` - Refresh authentication token

### Tasks

- `POST /api/tasks` - Create a new task
- `GET /api/tasks` - Retrieve all tasks
- `GET /api/tasks/{id}` - Retrieve a specific task
- `PUT /api/tasks/{id}` - Update a specific task
- `DELETE /api/tasks/{id}` - Delete a specific task

### Admin

- `GET /api/admin/{adminId}` - Retrieve admin details
- `GET /api/admin/current` - Retrieve current admin details

### Employees

- `GET /api/employees/{employeeId}` - Retrieve employee details
- `PUT /api/employees/{employeeId}` - Update employee details
- `DELETE /api/employees/{employeeId}` - Delete an employee

### Managers

- `GET /api/managers/{managerId}` - Retrieve manager details
- `PUT /api/managers/{managerId}` - Update manager details
- `DELETE /api/managers/{managerId}` - Delete a manager
