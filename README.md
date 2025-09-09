# 📦 SmartInventory

SmartInventory is a minimal inventory system built with **ASP.NET Core** and **EF Core**.  
It tracks products, atomic stock adjustments and movement history, provides a barcode scan API, optimistic concurrency control, background low-stock alerts, and authentication using **Identity + JWT**.  

It demonstrates real-world concepts like authentication, role-based authorization, service layers, and clean architecture.

---

## 🛠️ Tech Stack
- **.NET 8 / ASP.NET Core**
- **Entity Framework Core** (SQL Server / LocalDB)
- **ASP.NET Identity** (User + Role management)
- **JWT Authentication**
- **Swagger (OpenAPI)**
- **Dependency Injection**
- **Layered Architecture** (Services, Interfaces, Controllers)

---

## ✨ Features

### 🔐 Authentication
- Register & Login endpoints with **JWT Token**
- Role-based access (`ADMIN`, `USER`)
- Default seeded **Admin account**

### 📦 Inventory Management
- Add, update, and delete products
- Stock movement tracking (in/out/adjustments)
- Low-stock alerts

### 🌟 Extras
- Configurable connection string via `appsettings.json`
- Swagger UI for easy API testing
- Clean service-based architecture

---

## ⚙️ Setup Instructions

1. **Clone the repository**
   git clone https://github.com/Arslan193-ops/SmartInventory-System.git
   cd smartInventory-system

2. Update the database connection string in appsettings.json.

3. **Apply EF Core migrations**
dotnet ef database update

4. **Run the application**
dotnet run

5. Open **Swagger UI** at
https://localhost:7278/swagger.

**Default Credentials**
Admin
Email: admin@system.com
Password: Admin@123
