# 🚀 MiniStyle – E-Commerce Web Service

**MiniStyle E-Commerce Web Service** is a RESTful ASP.NET Core Web API designed to power the MiniStyle e-commerce platform. It provides secure and scalable APIs for managing products, categories, inventory, shopping carts, wishlists, orders, payments, customers, and discounts.

The API follows a clean layered architecture using Entity Framework Core, JWT authentication, role-based authorization, and logging.

## ✨ Features

- **Authentication & Authorization:** Secure JWT-based authentication and role-based access control.
- **Product Management:** Create, update, retrieve, and manage products.
- **Category Management:** Manage product categories.
- **Inventory Management:** Track and manage product stock.
- **Shopping Cart:** Add, update, and manage cart items.
- **Wishlist:** Manage customer wishlist items.
- **Order Management:** Handle customer orders and order processing.
- **Payment Management:** Manage payment-related operations.
- **Customer Management:** Manage customer information.
- **Discount Management:** Create and manage promotional discounts.
- **API Logging:** Maintain application logs for monitoring and troubleshooting.
- **Layered Architecture:** Organized and maintainable application structure.

## 🛠️ Technology Stack

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server (if configured as the database provider)
- JWT Authentication
- RESTful API
- LINQ
- Dependency Injection

## 🏗️ Project Architecture

The solution follows a layered architecture to separate responsibilities and improve maintainability.

```text
MiniStyle-ECommerce-Web-Service/
│
├── MiniStyleWebService.Domain/
│
├── MiniStyleWebService.Infrastructure/
│
├── MiniStyleWebService.Persistence/
│
├── MiniStyleWebService.Repository/
│
├── MiniStyleWebService.Service/
│
├── MiniStyleWebService/
│
└── MiniStyleWebService.sln
```

### Layer Description

| Project        | Responsibility                                           |
| -------------- | -------------------------------------------------------- |
| Domain         | Contains domain models and business entities.            |
| Infrastructure | Handles infrastructure-related functionality.            |
| Persistence    | Manages database context and persistence operations.     |
| Repository     | Handles data access and repository operations.           |
| Service        | Contains business logic and service operations.          |
| WebService     | Exposes RESTful API endpoints and handles HTTP requests. |

## 🔐 Security

- JWT-based authentication.
- Role-based authorization.
- Protected API endpoints.
- Secure configuration management.
- Centralized authentication and access control.

## ⚙️ Getting Started

### Prerequisites

- Visual Studio
- Compatible .NET SDK
- SQL Server (if configured)
- Entity Framework Core tools
- NuGet Package Manager

### Installation

**1. Clone the repository**

```bash
git clone https://github.com/drashti-shastra-soft/MiniStyle-ECommerce-Web-Service.git
```

**2. Navigate to the project directory**

```bash
cd MiniStyle-ECommerce-Web-Service
```

**3. Open the solution**

Open the following solution file in Visual Studio:

```text
MiniStyleWebService.sln
```

**4. Restore NuGet Packages**

```bash
dotnet restore
```

**5. Configure Application Settings**

Configure the required database connection string, JWT settings, and other application settings in the appropriate configuration files.

Do not commit production credentials, passwords, or JWT secrets to GitHub.

**6. Build the Project**

```bash
dotnet build
```

**7. Run the API**

Select the `MiniStyleWebService` startup project in Visual Studio and run the application.

## 🔗 Related Project

**MiniStyle E-Commerce Web Console**

The administration panel communicates with this Web API to manage the e-commerce platform.

Repository:
[https://github.com/drashti-shastra-soft/MiniStyle-ECommerce-Web-Console](https://github.com/drashti-shastra-soft/MiniStyle-ECommerce-Web-Console)

## 🎯 Project Objective

The objective of MiniStyle Web Service is to provide a secure, scalable, and maintainable backend solution for an e-commerce platform.

It centralizes business logic, database operations, authentication, and API communication for the MiniStyle ecosystem.

---

⭐ If you find this project useful, don't forget to star the repository!
