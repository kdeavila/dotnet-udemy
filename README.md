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
- **AutoMapper 14.0** - Object mapping
- **Swagger/OpenAPI** - API documentation
- **BCrypt.Net** - Password hashing
- **Dependency Injection** - Built-in IoT container

#### Features:
- Category management API
- Product management API
- User management with secure password handling
- Repository pattern implementation
- Automatic API documentation with Swagger
- Database migrations support
- Docker support (docker-compose.yaml included)

#### Project Structure:
```
ApiEcommerce/
├── Controllers/      - API endpoints
├── Data/            - Database context
├── Models/          - Entity models
├── Repository/      - Data access layer
├── Mapping/         - AutoMapper configurations
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
- ✅ Swagger/OpenAPI documentation
- ✅ Secure password handling with BCrypt
- ✅ Data transfer objects and mapping
- ✅ Reflection and custom attributes

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
- JWT settings (if applicable)
- API URLs
- Logging levels

## 📝 API Endpoints

The ApiEcommerce provides endpoints for:
- **Categories** - CRUD operations
- **Products** - CRUD operations with category relationships
- **Users** - User management and authentication

Detailed documentation available at `/swagger` when running in development mode.

## 🧪 Testing

Run tests for the entire solution:
```bash
dotnet test
```

## 📦 Dependencies

### CsBases
- .NET 10.0
- System libraries

### ApiEcommerce
- AutoMapper 14.0.0
- BCrypt.Net-Next 4.0.3
- Entity Framework Core 10.0.0
- SQL Server provider
- Swashbuckle (Swagger) 10.0.1
- OpenAPI support

## 🤝 Contributing

This is a learning repository. Feel free to explore, modify, and extend the examples.

## 📄 License

This project is part of a Udemy learning course.

---

**Happy Learning!** 🚀
