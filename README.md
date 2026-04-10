# ChurchMS — Church Management Solution

Multi-tenant church management platform built with ASP.NET Core Web API, Blazor Server admin dashboard, and .NET MAUI mobile app.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for SQL Server, Redis, Seq)
- [Visual Studio Code](https://code.visualstudio.com/) with C# Dev Kit extension

## Quick Start

### 1. Start Infrastructure

```bash
cd docker
docker-compose up -d
```

This starts:
- **SQL Server 2022** on port 1433
- **Redis 7** on port 6379
- **Seq** (log viewer) on port 8081 (UI) / 5341 (ingestion)
- **MinIO** (blob storage) on port 9000 / 9001 (console)

### 2. Apply Database Migrations

```bash
dotnet ef database update --project src/ChurchMS.Persistence --startup-project src/ChurchMS.API
```

### 3. Run the API

```bash
dotnet run --project src/ChurchMS.API/ChurchMS.API.csproj
```

API available at: `https://localhost:7110` | Swagger UI: `https://localhost:7110/swagger`

### 4. Run the Blazor Admin

```bash
dotnet run --project src/ChurchMS.BlazorAdmin/ChurchMS.BlazorAdmin.csproj
```

### 5. Build MAUI App

```bash
dotnet build src/ChurchMS.MAUI/ChurchMS.MAUI.csproj -f net10.0-android
```

## Running Tests

```bash
dotnet test                                    # all tests
dotnet test tests/ChurchMS.UnitTests/          # unit tests only
dotnet test tests/ChurchMS.IntegrationTests/   # integration tests (requires Docker)
```

## Project Structure

See [CLAUDE.md](CLAUDE.md) for full architecture documentation.

## Documentation

- [Functional Specification (EN)](docs/functional-spec-en.md)
- [Functional Specification (FR)](docs/functional-spec-fr.md)
