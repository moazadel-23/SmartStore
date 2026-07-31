# 🛒 SmartStore - Modern E-Commerce Platform

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-512BD4?style=for-the-badge&logo=nuget)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-blue.svg?style=for-the-badge)

## 📌 Project Overview
**SmartStore** is a feature-rich, high-performance e-commerce web application built using **ASP.NET Core 10.0 MVC**. Designed to provide a sophisticated and seamless shopping experience, the platform features a responsive design, advanced administrative tools, elegant customer-facing interfaces, and secure user authentication. It emphasizes clean coding practices, maintainability, and extensibility by leveraging the Generic Repository Pattern and built-in Localization for multilingual support.

---

## 🏗️ Architecture & Design Patterns
The project adheres to industry-standard architectural principles to guarantee scalability, testability, and clean code:

* **N-Tier MVC Pattern:** Separates concerns cleanly into Models, Views, and Controllers.
* **Area Routing:** Isolates specific business domains into logical areas:
  * `Admin`: Back-office panel for inventory, branding, category, and sales management.
  * `Customer`: Front-facing storefront including search, catalog, wishlist, shopping cart, and chatbot.
  * `Identity`: Authentication and role-based registration/login workflows.
* **Repository Pattern:** Utilizes a generic repository contract (`IRepository<T>`) to abstract data access logic, decoupling business rules from the underlying EF Core database operations.
* **Dependency Injection (DI):** Uses ASP.NET Core’s built-in DI container to register and resolve data repositories, database initializers, and services through a dedicated service layer configuration (`DI_Serice`).
* **Localization Middleware:** Dynamically reads culture information from cookies to serve localized resource files (`.resx`) for views, notifications, and data annotation validations.

---

## 🚀 Technologies Used
### Backend
* **Framework:** ASP.NET Core 10.0 MVC (target framework `net10.0`)
* **Language:** C# 13
* **ORM:** Entity Framework Core 10.0.7
* **Database:** Microsoft SQL Server
* **Security:** ASP.NET Core Identity (with configured password policies, role management, and cookie authentication)

### Frontend
* **UI Structure:** HTML5, CSS3, JavaScript (ES6)
* **Styling & Components:** Responsive Layouts, FontAwesome Icons, Premium CSS Effects
* **Client Scripts:** AJAX-based endpoints for floating interactive features

---

## ✨ Features
* 🔒 **Secure Authentication & Role-Based Access:** Standard ASP.NET Core Identity customized with role checks (`SuperAdmin` vs. `Customer`).
* 🤖 **Smart Chatbot Assistant:** A premium, interactive client-side floating widget capable of helping users with product queries, return policy, payment options, order tracking, and quick WhatsApp redirection.
* 🌍 **Localization / Multilingual Support:** Full native support for English (`en`) and Arabic (`ar`) languages. Culture switcher changes user experience dynamically across all areas.
* 📦 **Robust Catalog Management:** Full CRUD operations for:
  * **Categories:** Organized hierarchical tags.
  * **Brands:** Brand metadata and details.
  * **Products:** Multi-image uploads, specifications, pricing, reviews, and promotion associations.
* 🛍️ **Interactive Storefront:**
  * Dynamic live searching, filtering, and pagination.
  * Interactive Shopping Cart (add/remove, adjust quantities).
  * Wishlist/Favorites system.
* 🖥️ **Admin Dashboard:** High-end back-office analytics panel showcasing overall statistics, active promotions, carousel banner settings, and catalog status.
* 🌱 **Automatic Database Initialization:** Features a database initializer (`IDBInitilizer`) that automatically applies pending EF Core migrations and seeds initial application roles and admin account credentials on first startup.

---

## 📂 Project Structure

```text
SmartStore/
│
├── Areas/
│   ├── Admin/          # Admin controllers, views (Banners, Categories, Brands, Products, Promotions)
│   ├── Customer/       # User storefront controllers, views (Home, Wishlist, Chatbot)
│   └── Identity/       # Authentication area containing Account controllers and login/register views
│
├── Controllers/        # Global controllers (e.g., LocalizationController for culture management)
├── Models/             # Domain entities (Product, Brand, Category, Order, Cart, Favorite, etc.)
├── DataAccess/         # Entity Framework DbContext configuration (ApplicationDbContext)
├── Migrations/         # EF Core generated database schema migrations
├── Repositories/       # Data Access Layer using IRepository<T> & Repository<T> implementations
├── DI_Serice/          # Dependency Injection service configurations extension methods
├── Utilities/          # Seed Initializers, Roles (SD.cs), and constant definitions
├── ViewModel/          # DTOs and strongly-typed ViewModels
├── Views/              # Shared layouts (store _Layout, admin _AdminLayout) and partials
├── Resources/          # Locale files (resx) for Arabic and English translations
└── wwwroot/            # Static assets (custom CSS, JS, vendor libraries, uploaded images)
```

---

## 📸 Screenshots

### Home Page
![Home](images/home.png)

### Dashboard
![Dashboard](images/dashboard.png)

### Login
![Login](images/login.png)

### Details
![Details](images/details.png)

### Shopping Cart
![Shopping Cart](images/cart.png)

### Smart Chatbot Widget
![Smart Chatbot Widget](images/chatbot.png)

---

## 🎥 Demo

![Demo](images/demo.gif)

*The GIF demonstrates:*
* **Login**
* **CRUD Operations**
* **Search**
* **Filtering**
* **Dashboard**
* **Logout**

---

## ⚙️ Running the Project

Follow these step-by-step instructions to run the project locally on your machine.

1. **Clone the repository**
```bash
git clone <repository-url>
```

2. **Open the solution** in Visual Studio 2022.

3. **Update appsettings.json** with your SQL Server connection string.

Open the [appsettings.json](file:///c:/Users/2024/source/repos/SmartStore/SmartStore/appsettings.json) file and adjust the connection string under `ConnectionStrings.DefaultConnection`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=SmartStore;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=30;Command Timeout=30"
  }
}
```

4. **Open Package Manager Console** (Tools > NuGet Package Manager > Package Manager Console).

5. **Apply migrations** to create and initialize the SQL Server database:

```powershell
Update-Database
```

or if using the .NET CLI in your terminal:

```bash
dotnet ef database update
```

6. **Build the project** (`Ctrl + Shift + B` or Build > Build Solution).

7. **Run the project** (`F5` or `Ctrl + F5`).

8. **Open the browser** to launch the store interface.

9. **If Swagger exists**, navigate to:

```
https://localhost:xxxx/swagger
```
*(Note: As a classic MVC application, SmartStore utilizes standard controller-view routes for client interactions. Swagger is not configured out of the box).*

---

## 🔐 Sample Login Credentials
The application automatically seeds standard administrative credentials during the database initialization on the first run:

* **Admin Email:** `moazaboefadle@gmail.com`
* **Admin Username:** `MoazAdmin23`
* **Password:** `Moaz12345@`
* **Assigned Role:** `SuperAdmin`

---

## 🔮 Future Improvements
- [ ] **Payment Integration:** Implement Stripe and PayPal checkout flows.
- [ ] **Live Sales Analytics:** Add dynamic administrative charts and graphs using Chart.js & SignalR.
- [ ] **Caching Layer:** Integrate Redis Cache for high-traffic product catalog queries.
- [ ] **Web API Endpoints:** Build RESTful API endpoints protected by JWT tokens for companion mobile apps.
- [ ] **Email Alerts:** Add automatic order confirmation and delivery notifications via SendGrid/Twilio.

---

## 📄 License
This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute it for personal or commercial purposes.
