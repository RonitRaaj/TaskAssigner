# TaskAssigner API

A secure Task Management API built with .NET 10, Entity Framework Core, and JWT Authentication.

## 🚀 Features
- **User Authentication**: Register and Login using JWT (JSON Web Tokens).
- **Task Management**: CRUD operations for managing tasks.
- **SQLite Database**: Lightweight, file-based database with auto-migration on startup.
- **Global Exception Handling**: Centralized middleware to handle and format API errors.

## 🛠️ Tech Stack
- **Framework**: .NET 10
- **Database**: SQLite
- **ORM**: Entity Framework Core
- **Auth**: JWT Bearer Authentication

## ⚙️ Configuration
Update your `appsettings.json` with your JWT secrets:
```json
 "Jwt": {
    "Key": "YOUR_LONG_SECRET_KEY_HERE_32_CHARS",
    "Issuer": "SecurityVaultApi",
    "Audience": "SecurityVaultUsers"
  }
