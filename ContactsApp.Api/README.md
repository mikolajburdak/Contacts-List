# Contacts App Backend API

A simple ASP.NET Core Web API for managing contacts.

## Technologies

- ASP.NET Core (.NET 9.0)
- Entity Framework Core with SQLite
- ASP.NET Core Identity for authentication
- JWT Bearer Authentication
- Swagger/OpenAPI

## Features

- User registration and authentication
- Create, read, update, and delete contacts
- Category and subcategory management for contacts
- JWT token-based security

## Project Structure

- **Controllers/** - API endpoints
- **Services/** - Business logic implementation
- **Repositories/** - Data access layer
- **Models/** - Domain entities
- **DTO/** - Data transfer objects
- **Data/** - Database context and migrations

## Setup Instructions

1. Make sure you have .NET 9.0 SDK installed

2. Clone the repository

3. Navigate to the backend directory:
   ```
   cd ContactsApp.Api
   ```

4. Run the application:
   ```
   dotnet run
   ```

5. The API will be available at:
   ```
   http://localhost:5000
   ```

6. Swagger documentation is available at:
   ```
   http://localhost:5000/swagger
   ```

## API Endpoints

- **POST /api/auth/register** - Register a new user
- **POST /api/auth/login** - Login and get JWT token
- **GET /api/contact** - Get all contacts
- **GET /api/contact/{id}** - Get a specific contact
- **POST /api/contact** - Create a new contact (requires authentication)
- **PATCH /api/contact/{id}** - Update a contact (requires authentication)
- **DELETE /api/contact/{id}** - Delete a contact (requires authentication)

## Database

The application uses SQLite database stored in `contacts.db` file.