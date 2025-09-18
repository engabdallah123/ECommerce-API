# E-Commerce System API – ASP.NET Core (.NET 9)

A complete Web API project built with *ASP.NET Core (.NET 9)* following *Clean Architecture* principles. This system is designed to be the foundation of a full-featured E-Commerce platform with a focus on performance, scalability, and clean code structure.

---

## ⚙ Tech Stack

- ASP.NET Core Web API (.NET 9)
- Clean Architecture + SOLID Principles
- Entity Framework Core (Code First)
- Generic Repository + Unit of Work Pattern
- SQL Server
- JWT Authentication & Authorization
- Memory Cache
- AutoMapper
- FluentValidation
- Swagger / Postman

---

## ✨ Features

### User Management
- Upload profile image during registration
- Retrieve user image automatically on login

### Wallet System
- Add funds to wallet
- Withdraw from wallet
- Use wallet to pay for orders
- Real-time transaction logging and summaries

### Orders & Checkout
- Supports Wallet Payments & Cash on Delivery (COD)
- Each payment logs a transaction record with detailed info

### API Experience
- Full API documentation using *Swagger* and *Postman*
- *Pagination* for listing products and other resources
- *Global Exception Handling* and logging across the system

### Products & Categories
- Create, Read, Update, Delete products
- Pagination and filtering for frontend

### Admin Dashboard
- Manage products, users, orders, and customer messages
- Track state of orders
---

## 📁 Project Structure (Clean Architecture)
Core
|– Entities
|– Interfaces
|– Specifications

/Infrastructure
|– Data
|– Repositories
|– Configurations

/Application
|– Services
|– DTOs
|– Mappings
|– Validations

/API
|– Controllers
|– Middlewares
|– Endpoints

---

## 🔐 Security

- Secured APIs using JWT
- Role-based Authorization
- Protected wallet/payment endpoints

---


🤝 Contact

If you have any feedback, suggestions, or opportunities — feel free to connect with me here on GitHub or on LinkedIn.
www.linkedin.com/in/abdallah-ebrahim-5038272b6

---


#aspnetcore #dotnet #webapi #backenddeveloper #ecommerce #cleanarchitecture #csharp #restapi

---

#. Configure API
- Make sure the Angular app points to the backend API URL in `environment.ts`:
```ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5104/api'
};
