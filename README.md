# DotNet Udemy - Learning Repository

A comprehensive C# and .NET learning project containing practical examples of fundamental concepts and a complete e-commerce API application.

## 📋 Project Overview

This repository is divided into two main projects:

### 1. **CsBases** - C# Fundamentals Console Application
A console-based project demonstrating core C# and .NET concepts with practical examples.

#### Key Topics Covered:
- **02-Tipos-Basicos** - Basic types and data structures
- **04-Herencia** - Inheritance and OOP principles
- **05-Patron-Adaptador** - Adapter pattern implementation
- **06-Inyeccion-Dependencias** - Dependency injection patterns
- **07-Metodos-Asincronos** - Async/await programming
- **08-Atributos** - Custom attributes and reflection

#### Features:
- Product and ServiceProduct models
- DTO (Data Transfer Object) adaptation using the Adapter pattern
- Dependency injection with LabelService
- Asynchronous methods with async/await
- Custom attribute processors for data transformation

### 2. **ApiEcommerce** - RESTful E-Commerce API
A modern ASP.NET Core Web API for managing an e-commerce platform.

#### Technology Stack:
- **.NET 10.0** - Latest .NET framework
- **Entity Framework Core 10.0** - ORM with SQL Server
- **Mapster 7.4.0** - Object mapping
- **ASP.NET Core Identity** - User authentication and authorization
- **JWT Bearer Authentication** - Token-based security
- **API Versioning** - Version management for endpoints
- **Response Caching** - Performance optimization
- **CORS** - Cross-origin resource sharing
- **Swagger/OpenAPI** - API documentation
- **BCrypt.Net** - Password hashing
- **DotNetEnv** - Environment variables management
- **Dependency Injection** - Built-in IoC container

#### Features:
- Category management API (v1 and v2)
- Product management API
- User management with secure password handling
- User authentication and authorization with JWT tokens
- Role-based access control (Admin role)
- Repository pattern implementation
- API versioning support
- Response caching for performance
- CORS configuration
- Automatic API documentation with Swagger
- Database migrations support
- Docker support (docker-compose.yaml included)

#### Project Structure:
```
ApiEcommerce/
├── Constants/       - Cache profiles and policy names
├── Controllers/      - API endpoints (with versioning)
├── Data/            - Database context
├── Models/          - Entity models
├── Repository/      - Data access layer
├── Migrations/      - EF Core migrations
├── Properties/      - Application settings
└── appsettings*.json - Configuration files
```

## 🚀 Getting Started

### Prerequisites
- .NET 10.0 SDK or later
- SQL Server (for ApiEcommerce)
- Visual Studio 2022 or VS Code with C# extension

### Installation

1. **Clone the repository:**
```bash
git clone <repository-url>
cd dotnet-udemy
```

2. **Restore dependencies:**
```bash
dotnet restore
```

### Running CsBases (Console App)

```bash
cd CsBases
dotnet run
```

This will execute the fundamentals examples demonstrating:
- Object instantiation and polymorphism
- Adapter pattern usage
- Dependency injection
- Async operations with product repository

### Running ApiEcommerce (Web API)

1. **Update database connection** in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "SqlConnection": "Server=YOUR_SERVER;Database=ApiEcommerce;Trusted_Connection=true;"
  }
}
```

2. **Apply database migrations:**
```bash
cd ApiEcommerce
dotnet ef database update
```

3. **Run the API:**
```bash
dotnet run
```

4. **Access Swagger UI:**
Navigate to `https://localhost:5001/swagger` (port may vary)

5. **Authentication:**
- Register a new user via `/api/v1.0/users/register`
- Login to get JWT token via `/api/v1.0/users/login`
- Use the token in Swagger by clicking "Authorize" button

### Running with Docker

```bash
docker-compose up
```

## 📚 Learning Outcomes

This project demonstrates:
- ✅ Object-oriented programming with C#
- ✅ Design patterns (Adapter, Repository, Dependency Injection)
- ✅ Async/await programming model
- ✅ Entity Framework Core with migrations
- ✅ RESTful API design principles
- ✅ ASP.NET Core Identity for authentication
- ✅ JWT token-based security
- ✅ API versioning strategies
- ✅ Response caching implementation
- ✅ CORS configuration
- ✅ Swagger/OpenAPI documentation with security
- ✅ Secure password handling with BCrypt
- ✅ Data transfer objects and mapping
- ✅ Reflection and custom attributes
- ✅ Role-based authorization

## 🏗️ Project Structure

```
dotnet-udemy/
├── CsBases/
│   ├── Fundamentals/        - Core examples
│   ├── Program.cs           - Console entry point
│   └── CsBases.csproj
├── ApiEcommerce/
│   ├── Controllers/         - API endpoints
│   ├── Data/               - Database context
│   ├── Models/             - Entity models
│   ├── Repository/         - Data layer
│   ├── Program.cs          - Web API entry point
│   └── ApiEcommerce.csproj
├── DotnetUdemy.sln         - Solution file
└── README.md
```

## 🔧 Configuration

### ApiEcommerce Settings

**appsettings.json** - Production settings
**appsettings.Development.json** - Development settings with detailed logging

Key configurations:
- Database connection strings
- JWT settings (SecretKey for token signing)
- API URLs
- Logging levels
- CORS origins

## 📝 API Endpoints

The ApiEcommerce provides endpoints for:
- **Categories** - CRUD operations (v1 and v2 with ordering improvements)
- **Products** - CRUD operations with category relationships
- **Users** - User management, registration, and authentication

### Authentication
- **POST** `/api/v1.0/users/register` - User registration
- **POST** `/api/v1.0/users/login` - User login (returns JWT token)

### Categories
- **GET** `/api/v1.0/categories` - Get all categories (deprecated)
- **GET** `/api/v2.0/categories` - Get all categories ordered by ID
- **GET** `/api/v2.0/categories/{id}` - Get category by ID
- **POST** `/api/v2.0/categories` - Create new category
- **PUT** `/api/v2.0/categories/{id}` - Update category
- **DELETE** `/api/v2.0/categories/{id}` - Delete category

### Products
- **GET** `/api/v1.0/products` - Get all products
- **GET** `/api/v1.0/products/{id}` - Get product by ID
- **POST** `/api/v1.0/products` - Create new product
- **PUT** `/api/v1.0/products/{id}` - Update product
- **DELETE** `/api/v1.0/products/{id}` - Delete product

### Users (Admin only)
- **GET** `/api/v1.0/users` - Get all users
- **GET** `/api/v1.0/users/{id}` - Get user by ID

Detailed documentation available at `/swagger` when running in development mode.

## 📦 Dependencies

### CsBases
- .NET 10.0
- System libraries

### ApiEcommerce
- AutoMapper 14.0.0
- BCrypt.Net-Next 4.0.3
- Entity Framework Core 10.0.0
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 10.0.0
- Microsoft.AspNetCore.Authentication.JwtBearer 10.0.0
- Asp.Versioning.Mvc.ApiExplorer 8.1.0
- Mapster 7.4.0
- DotNetEnv 3.1.1
- Swashbuckle (Swagger) 10.1.0
- OpenAPI support

---

**Happy Learning!** 🚀
