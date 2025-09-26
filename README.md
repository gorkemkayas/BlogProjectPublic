📖 BlogProject - Comprehensive Documentation

[![.NET](https://img.shields.io/badge/.NET-8-blue)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Live Demo](https://img.shields.io/badge/Live-Demo-blue)](https://kayas.dev)

A **modern and scalable blog platform** built with **.NET 8** and **Razor Pages**.  
Includes categories, posts, user management, and an admin panel with a **responsive UI** and excellent **user experience**.

---

## 📑 Table of Contents
- [Overview](#-overview)
- [Technologies & Architecture](#-technologies--architecture)
- [Installation & Setup](#-installation--setup)
- [Configuration](#-configuration)
- [Main Modules & Features](#-main-modules--features)
- [Database Structure](#-database-structure)
- [Security](#-security)
- [Development & Contribution](#-development--contribution)
- [Deployment](#-deployment)
- [Troubleshooting](#-troubleshooting)
- [License](#-license)

---

## 🔎 Overview
**BlogProject** is a lightweight, flexible, and powerful blogging system.  
Can be deployed to **IIS, Azure, or Linux servers**. Supports modularity, scalability, and modern web standards.

**Live Demo:** [kayas.dev](https://kayas.dev)

---

## 🛠 Technologies & Architecture
| Layer       | Technology Stack |
|------------|----------------|
| Backend    | .NET 8, Razor Pages, Entity Framework Core |
| Frontend   | HTML5, CSS3 (Bootstrap), JavaScript |
| Database   | SQL Server (or EF Core compatible RDBMS) |
| Session    | ASP.NET Core Session |
| Authentication | ASP.NET Core Identity (optional) |
| Email      | SMTP |
| Security   | ASP.NET Core Data Protection, HTTPS/HSTS |

---

## ⚙️ Installation & Setup

### Requirements
- .NET 8 SDK  
- Visual Studio 2022  
- SQL Server (or compatible DB)

### Steps
1. Restore dependencies:
```bash
dotnet restore
```
2. Apply database migrations:
```bash
dotnet ef database update
```
3. Run the application:
```bash
dotnet run
```

## ⚡ Configuration

SMTP Settings (appsettings.json):
```bash
"EmailSettings": {
  "Host": "smtp.example.com",
  "Port": 587,
  "Username": "your@email.com",
  "Password": "yourpassword"
}
```
Data Protection: Keys stored in App_Data/keys. Use SetApplicationName for environment isolation.

Session: Add app.UseSession() in Program.cs.

## 📦 Main Modules & Features
Category Management

List categories and posts

Supports grid, list, and masonry layouts

Post Management

Add posts with drag-drop cover image upload and preview

Draft/publish toggle, edit, and delete posts

User Management (Admin)

List, activate, delete users

Modal confirmations and tooltips

Comments (Optional)

Users can comment on posts

Admin can manage comments

Error Management

Detailed error pages in development

Custom error pages + HSTS in production

Centralized ExceptionMiddleware

Session & Authentication

User sessions and role-based access control

Secure admin panel authorization

## 🗄 Database Structure

Tables:

Users → User info & roles

Posts → Title, content, cover image, status

Categories → Name & description

Comments (optional) → Text, linked to posts/users

Relationships:

1 Category → Many Posts

1 Post → Many Comments

1 User → Many Posts/Comments

## 🔒 Security

Secure key storage with Data Protection

Identity, roles, and session management

HTTPS & HSTS enforced in production

OWASP Top 10 secure coding practices followed

## 🤝 Development & Contribution

Workflow:

Fork the repository & create a branch

Make changes & test

Submit a Pull Request

Code Standards:

Follow C# 12 and .NET 8 standards

Razor Pages architecture

Include inline comments & documentation

## 🚀 Deployment

Build for production:
```bash
dotnet publish -c Release
```
Deploy to IIS, Azure, or Linux server

Use environment variables for SMTP and database connections

## 🛠 Troubleshooting

Migration Errors → Check dependencies & connection string

SMTP Errors → Verify host, port, credentials

Session Errors → Ensure app.UseSession() is configured

Static Files Issues → Ensure app.UseStaticFiles() is added
