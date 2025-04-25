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

### Shopping Cart
- In-memory Cart using *Memory Cache* for ultra-fast response
- Built with interfaces + *Dependency Inversion Principle*

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

## 🛠 Postman Collection

You can test the full API using the provided Postman collection.  
Just import the file into Postman and start exploring the endpoints.

1. *Download the Postman collection:*

https://github.com/engabdallah123/ECommerce-API/blob/master/Postman/E_Commerce%20-%20Infinity.postman_collection.json

2. *go to Postman as viewer:*
      https://abdallahebrahim-4906911.postman.co/workspace/Abdallah-Ebrahim's-Workspace~9384c3f3-9678-4144-8585-15aa9d1d19a6/collection/43670293-e576129b-03d4-4c74-aba4-6ee860e3cdc6?action=share&creator=43670293
  
4. *How to use it in Postman:*
   - Open *Postman*
   - Click *Import* > *Choose Files*
   - Select the downloaded file
   - You’ll see all the endpoints organized by categories (auth, cart, wallet, orders, etc.)

---

Once imported, you can test:
- User Registration & Login (with image upload)
- Cart operations (Add/Remove products)
- Wallet transactions (Add, Withdraw, Pay)
- Order checkout with full transaction logging
⸻

🤝 Contact

If you have any feedback, suggestions, or opportunities — feel free to connect with me here on GitHub or on LinkedIn.
www.linkedin.com/in/abdallah-ebrahim-5038272b6


⸻

#aspnetcore #dotnet #webapi #backenddeveloper #ecommerce #cleanarchitecture #csharp #restapi
