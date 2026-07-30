<div align="center">
  <h1>📱 Phone Service System</h1>
  <p>A robust, desktop-based management system for handling telecommunication clients, services, and subscriptions.</p>

  <!-- Badges -->
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/.NET_Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET">
  <img src="https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white" alt="SQLite">
  <img src="https://img.shields.io/badge/Windows_Forms-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows Forms">
</div>

---

## 📖 Overview

The **Phone Service System** is a Windows Forms (WinForms) application developed in C# that allows businesses to efficiently manage their telecommunication services. It provides an intuitive user interface for managing clients, tracking extra service options, and maintaining active subscriptions. The application leverages a local **SQLite database** to ensure fast, reliable, and persistent data storage without the need for a complex server setup.

This project demonstrates strong object-oriented programming principles, the Repository design pattern, and effective use of the ADO.NET/SQLite data access layer.

## 📚 Documentation & Wiki

For detailed guides on how to use the application and a deep dive into the database architecture, please visit the **[Project Wiki](https://github.com/danielharton/PhoneServiceSystem/wiki)**. 


## ✨ Key Features

- **👥 Client Management**: Create, read, update, and delete (CRUD) client profiles, including first name, last name, and contact numbers.
- **⚙️ Service Options**: Manage available add-on services (e.g., extra data, international calling) along with their monthly costs.
- **📅 Subscription Tracking**: Assign service options to clients, complete with start and end dates.
- **💾 Local Data Persistence**: Uses a lightweight, embedded SQLite database (`database.db`) initialized automatically on startup.
- **📤 Data Export**: Export client and subscription data to a neatly formatted `.txt` file for reporting and external use.
- **🎨 Intuitive UI**: A clean, multi-window Windows Forms interface with list views, contextual dialogs, and standard menu navigation.

## 🛠️ Technology Stack

- **Language:** C# 7.0+
- **Framework:** .NET Framework 4.7.2
- **UI Architecture:** Windows Forms (WinForms)
- **Database:** SQLite (via `System.Data.SQLite`)
- **ORM / Data Access:** Raw ADO.NET with parameterized queries for maximum performance and security against SQL injection.

## 🏗️ Architecture & Design Patterns

- **Repository Pattern**: The `Repository.cs` class abstracts all direct database interactions. This centralizes SQL queries, simplifies the UI code, and makes the application easier to maintain and test.
- **Parameterized Queries**: All database inputs use parameterized SQL statements to prevent SQL injection vulnerabilities.
- **Event-Driven UI**: Forms utilize standard event handlers (`Click`, `Load`) to manage state and interact with the data layer.

## 🚀 Getting Started

### Prerequisites
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or Visual Studio 2019)
- .NET Framework 4.7.2 Developer Pack

### Installation & Execution
1. Clone the repository to your local machine.
2. Open the solution file `PhoneServiceSystem.sln` in Visual Studio.
3. Restore NuGet packages (Right-click the Solution in Solution Explorer -> **Restore NuGet Packages**).
4. Press `F5` or click **Start** to run the application. 
5. *Note: The SQLite database (`database.db`) will be created automatically in the `bin/Debug` directory upon first launch.*

## 📂 Project Structure

```text
PhoneServiceSystem/
│
├── ClassFolder/
│   ├── Client.cs             # Client entity model
│   ├── ExtraOption.cs        # Service option entity model
│   ├── Subscription.cs       # Subscription entity model
│   └── Repository.cs         # SQLite database access layer (CRUD operations)
│
├── FormFolder/
│   ├── MainForm.cs           # Main dashboard and navigation
│   ├── ClientForm.cs         # Client management list view
│   ├── ClientEditDialog.cs   # Dialog for adding/editing a client
│   ├── SubscriptionForm.cs   # Subscription management list view
│   └── ExtraOptionForm.cs    # Service options management list view
│
├── App.config                # Application configuration
└── packages.config           # NuGet package dependencies
```

## 💡 What Makes This Project Stand Out?

- **Clean Code & Separation of Concerns**: By keeping database logic out of the UI forms (via `Repository.cs`), the codebase is scalable and readable.
- **Zero-Configuration Database**: The choice of SQLite means reviewers and users don't need to install SQL Server or configure connection strings to run the app. It works right out of the box.
- **Robust Error Handling**: Safely exports data with exception handling and utilizes `using` statements for `SQLiteConnection` and `SQLiteCommand` to ensure proper disposal of unmanaged resources.

---
*Thank you for reviewing my project! Feel free to reach out if you have any questions about the implementation.*
