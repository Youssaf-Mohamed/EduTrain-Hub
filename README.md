<p align="center">
  <img src="wwwroot/images/logo.png" alt="EduTrain Hub Logo" width="220" />
</p>

<h1 align="center">🎓 EduTrain Hub</h1>
<p align="center">
  <strong>An Advanced & Enterprise-Grade Training Management System (TMS)</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0_MVC-blueviolet?style=for-the-badge&logo=.net" alt=".NET 9.0" />
  <img src="https://img.shields.io/badge/Database-SQLite-003B57?style=for-the-badge&logo=sqlite" alt="SQLite" />
  <img src="https://img.shields.io/badge/Security-Dynamic_RBAC-red?style=for-the-badge&logo=auth0" alt="RBAC Security" />
  <img src="https://img.shields.io/badge/Frontend-Bootstrap_&_VanillaJS-563D7C?style=for-the-badge&logo=bootstrap" alt="Bootstrap" />
</p>

---

## 🌟 Overview
**EduTrain Hub** is a state-of-the-art educational and training management system designed to streamline academic administrative workflows, track course performance, and secure enterprise operations. Built on top of the robust **ASP.NET Core 9 (MVC)** architecture, it delivers exceptional performance, high responsiveness, and production-grade security. 

With a dynamic permission management engine, automated seeder initializations, and an integrated analytics dashboard, it provides a comprehensive end-to-end portal for administrators, instructors, and trainees.

---

## ✨ Key Capabilities & Modules

### 🛡️ 1. Dynamic RBAC Security System
*   **Granular Access Control:** Implements a dynamic claims-based permission scheme mapping roles to fine-grained controller actions. Permissions are managed at the database level and verified dynamically on every request.
*   **Cookie Authentication:** Secure custom Cookie Authentication mechanism (`TrainMS.Auth`) with sliding expiration, custom expiration timespan, and automatic login/access-denied redirection logic.
*   **Protected System Roles:** Role hierarchy protection with immutable database seeder setups to secure default administrative access.

### 📊 2. Performance Analytics Dashboard
*   **Student Performance Tracking:** Provides detailed insights, student levels, grade distributions, and GPA/result metrics.
*   **Academic KPIs:** Visualizes department sizes, instructor-to-student ratios, course success percentages, and active enrollment rates.
*   **Modern Visual Representation:** Employs sleek, modern styling with dynamic visual layouts tailored for comprehensive reporting.

### 👥 3. Instructors & Trainees Portals
*   **Smart Instructor Directory:** An interactive search console with advanced filters for specialized tracks, department assignments, and experience levels.
*   **Comprehensive Student Profiles:** Tracks course registration status, active cohorts, and ongoing certifications.

### 📚 4. Course Management & Results Hub
*   **Curriculum Builder:** Creation, updates, and credit hour allocations for multiple specialized educational tracks.
*   **Grades Engine:** Automated course result entries, passing score validation, and direct grade reporting sheet templates.

---

## 🛠️ Architecture & Tech Stack

*   **Framework:** ASP.NET Core 9.0 (MVC Pattern)
*   **Data Access:** Entity Framework Core (EF Core) with LINQ
*   **Database Engine:** SQLite (Local development friendly, auto-migrating on startup)
*   **Asset Management:** .NET 9 Optimized Static Assets pipeline (`MapStaticAssets`)
*   **Authorization:** Custom Dynamic Policies & Role Claims Validation
*   **Styling:** Modern Vanilla CSS + Bootstrap, curated dark/light UI schemes, sleek transitions, and premium responsive layouts.

---

## 📂 Project Structure

```text
├── 📂 Controllers        # Handles all routing, actions, and API operations
├── 📂 Data               # Database context, entity configurations, and Auth seeder
├── 📂 Migrations         # EF Core SQLite database schema migration files
├── 📂 Models             # Core domain models (Course, Trainee, Instructor, etc.)
├── 📂 Security           # Permission configurations and auth custom extensions
├── 📂 Services           # Business logic layers (e.g. CurrentUserService)
├── 📂 ViewModels         # MVVM views representation models (Analytics, Search)
├── 📂 Views              # Razor views (.cshtml) including interactive dashboards
└── 📂 wwwroot            # Static client-side assets (optimized CSS, JS, premium logo)
```

---

## 🚀 Installation & Local Run

To run **EduTrain Hub** on your local machine, follow these steps:

### Prerequisites
*   [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed.
*   Visual Studio 2022 (v17.12+) or VS Code.

### Step-by-Step Run
1.  **Clone the Repository:**
    ```bash
    git clone https://github.com/Youssaf-Mohamed/EduTrain-Hub.git
    cd "Windows Programing"
    ```
2.  **Restore NuGet Packages:**
    ```bash
    dotnet restore
    ```
3.  **Run Migrations & Seed Database:**
    The application automatically triggers `context.Database.MigrateAsync()` and runs `AuthSeeder` on startup to deploy the SQLite database and seed roles/permissions dynamically.
4.  **Run the Application:**
    ```bash
    dotnet run
    ```
5.  **Access in Browser:**
    Open [https://localhost:7198](https://localhost:7198) or [http://localhost:5246] (or the port specified in your console).

---

## 🔒 Default Credentials
Upon initial startup, the database is seeded automatically with the following credentials for testing:
*   **Username / Email:** `admin@edutrain.com`
*   **Password:** `Admin@123`

---

<p align="center">
  Made with ❤️ | 2026
</p>
