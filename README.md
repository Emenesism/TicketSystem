# TicketSystem

A full-stack support ticket management system built with ASP.NET Core 10.0, featuring role-based authentication, secure session management, and a clean architecture design.

## Tech Stack

- **Backend**: ASP.NET Core 10.0 Web API
- **Database**: PostgreSQL with Entity Framework Core 10.0
- **Authentication**: JWT Bearer Tokens with Refresh Token Rotation
- **Architecture**: Clean Architecture (Domain → Application → Infrastructure → API)
- **Frontend**: HTML/CSS/JavaScript
- **Testing**: xUnit

## Features

- **User Management**: Registration, login, and profile updates
- **Admin Panel**: Role-based admin access (Admin, SuperAdmin)
- **Ticket System**: Create, assign, and resolve support tickets
- **Messaging**: Send and receive messages within tickets
- **Authentication**: JWT access tokens with secure refresh token rotation
- **Session Management**: Track active sessions with reuse detection for security
- **Dashboard**: Admin statistics and ticket overview
- **CORS**: Configured for frontend integration

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      TicketSystem.Api                       │
│              (Controllers, Middleware, Startup)             │
├─────────────────────────────────────────────────────────────┤
│              TicketSystem.Application                       │
│           (DTOs, Interfaces, Exceptions)                    │
├─────────────────────────────────────────────────────────────┤
│              TicketSystem.Infrastructure                    │
│        (Repositories, EF Core, JWT, Security)               │
├─────────────────────────────────────────────────────────────┤
│                  TicketSystem.Domain                        │
│                   (Entities, Models)                        │
└─────────────────────────────────────────────────────────────┘
```

## Project Structure

```
TicketSystem/
├── src/
│   ├── TicketSystem.Api/              # Web API entry point
│   │   ├── Controllers/               # API endpoints
│   │   │   ├── AuthControllers.cs     # Authentication endpoints
│   │   │   ├── UsersControllers.cs    # User management
│   │   │   ├── AdminControllers.cs    # Admin operations
│   │   │   ├── TicketsControllers.cs  # Ticket CRUD
│   │   │   ├── MessagesControllers.cs # Message operations
│   │   │   ├── DashboardControllers.cs# Admin dashboard
│   │   │   └── HealthzControllers.cs  # Health check
│   │   ├── Middleware/                # Exception handling
│   │   ├── Extensions/                # Auto-migration
│   │   └── program.cs                 # Application entry
│   │
│   ├── TicketSystem.Application/      # Application layer
│   │   ├── Abstractions/Repositories/ # Repository interfaces
│   │   ├── Dtos/                      # Request/Response DTOs
│   │   └── Common/                    # Exceptions, errors
│   │
│   ├── TicketSystem.Domain/           # Domain layer
│   │   └── Entities/                  # Domain models
│   │       ├── User.cs                # User entity
│   │       ├── Admin.cs               # Admin entity
│   │       ├── Ticket.cs              # Ticket entity
│   │       ├── Message.cs             # TicketMessage entity
│   │       └── Session.cs             # Session entity
│   │
│   ├── TicketSystem.Infrastructure/   # Infrastructure layer
│   │   ├── Persistance/               # Database layer
│   │   │   ├── Configuration/         # DbContext, mappings
│   │   │   ├── Repositories/          # Repository implementations
│   │   │   └── Migrations/            # EF Core migrations
│   │   └── Security/                  # JWT, password hashing
│   │
│   └── TicketSystem.Frontend/         # Frontend files
│       ├── index.html                 # Login page
│       ├── dashboard.html             # User dashboard
│       └── admin.html                 # Admin panel
│
└── test/
    └── TicketSystem.Api.Tests/        # API tests
```

## Domain Entities

| Entity | Description |
|--------|-------------|
| **User** | End users who can create tickets |
| **Admin** | Support staff with role-based permissions |
| **Ticket** | Support tickets with title, status, and assignment |
| **TicketMessage** | Messages within a ticket conversation |
| **Session** | Authentication sessions for refresh tokens |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (v12+)
- [Git](https://git-scm.com/downloads)

## Setup & Installation

### 1. Clone the Repository

```bash
git clone <repository-url>
cd TicketSystem
```

### 2. Configure Database Connection

Create an `appsettings.json` file in the `TicketSystem.Api` project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ticketsystem;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Issuer": "TicketSystem",
    "Audience": "TicketSystem",
    "Key": "your-super-secret-key-minimum-32-characters-long"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Apply Database Migrations

Migrations are applied automatically on startup. To apply manually:

```bash
cd src/TicketSystem.Api
dotnet ef database update
```

### 5. Run the Application

```bash
cd src/TicketSystem.Api
dotnet run
```

The API will be available at `https://localhost:5001` (or the configured URL).

## API Endpoints

### Authentication (`/auth`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/auth/user/login` | Login or register a user | No |
| POST | `/auth/admin/login` | Admin login | No |
| POST | `/auth/user/refresh` | Refresh user access token | No (cookie) |
| POST | `/auth/admin/refresh` | Refresh admin access token | No (cookie) |
| POST | `/auth/logout` | Logout and revoke session | No (cookie) |
| POST | `/auth/admin/create` | Create new admin | SuperAdmin |
| POST | `/auth/admin/revoke-all` | Revoke all admin sessions | Admin |
| POST | `/auth/user/revoke-all` | Revoke all user sessions | User |

### Tickets (`/ticket`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/ticket` | Create a new ticket | User |
| GET | `/ticket/user` | Get user's tickets | User |
| GET | `/ticket/admin` | Get admin's assigned tickets | Admin |
| GET | `/ticket/not-assign` | Get unassigned tickets | Admin |
| POST | `/ticket/solve` | Mark ticket as solved | Admin |
| POST | `/ticket/assign` | Assign ticket to admin | Admin |

### Messages (`/message`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/message` | Send a message in a ticket | User/Admin |
| GET | `/message/{ticketId}` | Get messages for a ticket | User/Admin |

### Users (`/user`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/user` | Get current user profile | User |
| PUT | `/user` | Update user profile | User |

### Admin (`/admin`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/admin` | Get current admin profile | Admin |
| PUT | `/admin` | Update admin profile | Admin |

### Dashboard (`/dashboard`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/dashboard` | Get admin statistics | Admin |

### Health Check

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/healthz` | Application health check |

## Security Features

- **JWT Authentication**: Stateless token-based authentication
- **Refresh Token Rotation**: Secure token refresh with rotation
- **Session Tracking**: Track active sessions with device/IP info
- **Reuse Detection**: Automatically revoke all sessions if token reuse is detected
- **Password Hashing**: Secure password storage using ASP.NET Identity hasher
- **Role-Based Authorization**: User, Admin, and SuperAdmin roles
- **CORS Policy**: Configured for specific frontend origins

## Running Tests

```bash
dotnet test test/TicketSystem.Api.Tests/TicketSystem.Api.Tests.csproj
```

## Development

### Adding a New Migration

```bash
cd src/TicketSystem.Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../TicketSystem.Api
```

### Code Style

- Nullable reference types enabled
- Implicit usings enabled
- Target framework: .NET 10.0

## License

This project is for educational purposes.
