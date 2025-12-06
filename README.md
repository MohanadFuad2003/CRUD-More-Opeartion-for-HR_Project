# HRSYS – Human Resources Management System

HRSYS is a layered ASP.NET solution for managing HR operations.  
The system is split into multiple projects (API, MVC Web UI, Domain, Application, Infrastructure) following clean architecture principles and sharing a common domain layer.

---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Projects Structure](#projects-structure)
- [Core Features](#core-features)
- [Technologies Used](#technologies-used)
- [Authentication (JWT)](#authentication-jwt)
- [Web UI (MVC + cshtml)](#web-ui-mvc--cshtml)
- [Static Files (wwwroot)](#static-files-wwwroot)
- [Domain & DTOs Sharing](#domain--dtos-sharing)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Configuration](#configuration)
  - [Database Setup](#database-setup)
  - [Running the Solution](#running-the-solution)
- [Development Guidelines](#development-guidelines)
- [Future Improvements](#future-improvements)

---

## Architecture Overview

The solution follows a **layered / clean architecture** approach:

- **Domain Layer**: Contains the core business model (Entities, Enums, Value Objects, DTOs).
- **Application Layer**: Contains application logic (use cases, services, interfaces).
- **Infrastructure Layer**: Responsible for persistence, external services, and concrete implementations.
- **API Layer**: Exposes RESTful endpoints using ASP.NET controllers and JWT authentication.
- **MVC Web Layer (WEB-MCV)**: Provides the web user interface (Views with `.cshtml`, controllers, static resources in `wwwroot`).

The main goal is to keep the **business rules isolated** and reuse models/DTOs across both the API and the MVC application.

---

## Projects Structure

The solution contains the following projects:

1. **HRSYS.Domain**
   - Contains:
     - Domain entities (e.g. Employee, Department, Attendance, etc.)
     - DTOs shared between the API and MVC projects.
     - Core business rules and validations.
   - Has no dependency on other layers (clean architecture core).

2. **HRSYS.Application**
   - Contains:
     - Application services / use cases.
     - Interfaces (e.g. repositories, external service ports) to be implemented in `Infrastructure`.
     - Mapping between domain entities and DTOs where needed.

3. **HRSYS.Infrastructure**
   - Implements:
     - Data access (e.g. EF Core DbContext, repositories).
     - Persistence logic for domain entities.
     - Integrations (email, file storage, external APIs) if applicable.
   - References `Domain` and `Application` layers.

4. **HRSYS.API**
   - ASP.NET (Core) Web API.
   - Contains controllers that:
     - Use application services.
     - Return DTOs defined in the shared Domain/Application layer.
   - Uses **JWT tokens** for authentication & authorization.
   - Typically acts as a backend for external clients (SPA, mobile, integrations).

5. **WEB-MCV**
   - ASP.NET MVC web application.
   - Contains:
     - MVC controllers.
     - Views (`.cshtml` files).
     - `wwwroot` folder for static assets (CSS, JS, images, etc.).
   - Reuses DTOs / models from the shared Domain layer.
   - Can communicate with:
     - API layer (HRSYS.API) via HTTP calls, or
     - Directly with Application/Infrastructure (depending on design).

---

## Core Features

> The exact features depend on your implementation. Below is a generic list you can adjust:

- Employee management (create, update, list, deactivate).
- Departments and positions management.
- Attendance and leave tracking.
- Role-based access control (via JWT claims/roles).
- Web dashboard for HR admins and employees.
- RESTful API endpoints for integration with other systems.

Update this section with your exact modules once finalized.

---

## Technologies Used

- **.NET / ASP.NET Core**
- **ASP.NET MVC** (`WEB-MCV` project)
- **ASP.NET Web API** (`HRSYS.API` project)
- **JWT (JSON Web Token)** for authentication
- **cshtml** Razor views
- **wwwroot** for static files (CSS, JS, images)
- **Entity Framework Core** (if used – adjust as needed)
- **SQL Server** or any other RDBMS (update with your DB)
- **Dependency Injection** (built-in .NET DI container)
- **DTOs shared across layers**

---

## Authentication (JWT)

The system uses **JWT tokens** to authenticate and authorize API requests:

- Users log in (e.g. via `/api/auth/login`) with username/password.
- On success, the API returns a **JWT token**.
- The client (MVC app, frontend, or external client) sends the token in the `Authorization` header:
  ```http
  Authorization: Bearer <token>
