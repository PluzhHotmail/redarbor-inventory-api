# Inventory API – CQRS (.NET)

RESTful API for inventory management implemented using **Clean Architecture** and **CQRS**, designed to run locally using **Docker** and **Docker Compose**.

---

## Technologies

- .NET 6
- ASP.NET Core Web API
- CQRS (Commands / Queries)
- SQL Server 2022
- Dapper (Write side)
- Entity Framework Core (Read side)
- JWT Authentication
- Swagger / OpenAPI
- Docker / Docker Compose
- xUnit / Moq (Unit Testing)

---

## Architecture

```
Inventory.Api                 → Controllers / Endpoints
Inventory.Application         → Commands, Queries, DTOs, Interfaces
Inventory.Domain              → Domain entities and business rules
Inventory.Infrastructure      → Persistence (EF Core / Dapper)
Inventory.Tests               → Unit tests (Application layer)
```

Applied principles:
- Clean Architecture
- CQRS (separation of read and write models)
- SOLID principles
- Soft delete using `Status` field
- Inventory movements (entry / exit)
- Domain fully decoupled from infrastructure

---

## Prerequisites

- Docker
- Docker Compose
- Git

> It is **not required** to have .NET or SQL Server installed locally.

---

## Local setup

### 1. Clone repository

```bash
git clone https://github.com/PluzhHotmail/redarbor-inventory-api.git
cd redarbor-inventory-api
```

---

### 2. Environment variables

Review the file:

```
docker-compose.yml
```

Example SQL Server configuration:

```yaml
environment:
  SA_PASSWORD: "YourStrong!Passw0rd"
  ACCEPT_EULA: "Y"
```

You may change the password if needed.

---

## Run the application

From the project root:

```bash
docker compose up --build
```

Running services:
- API (`inventory-api`)
- SQL Server (`inventory-sqlserver`)

---

## Application access

### API
```
http://localhost:5000
```

### Swagger
```
http://localhost:5000/swagger
```

---

## Unit tests

Unit tests are implemented focusing on the **Application layer**, where business logic resides, following Clean Architecture and CQRS best practices.

### Scope

- Command Handlers
- Query Handlers

Controllers and infrastructure components are intentionally excluded from unit testing, as they are better suited for integration tests.

### Test project

```
Inventory.Application.Tests
```

Main tested components:
- CreateCategoryCommandHandler
- CreateProductCommandHandler
- RegisterInventoryMovementCommandHandler
- GetProductsQueryHandler

### Run unit tests

```bash
dotnet test
```

---

## API testing with Postman

The repository includes a **Postman collection** with the main API endpoints for manual testing.

### Location

```
/postman/Inventory.postman_collection.json
```

### Usage

1. Open Postman
2. Import the collection:
   - `Import` → `File` → select `Inventory.postman_collection.json`
3. Configure environment variables:
   - `baseUrl` → `https://localhost:5000`
   - `token` → JWT token value
4. Execute requests from the collection

Included examples:
- Authentication (JWT)
- Category CRUD
- Product CRUD
- Inventory movements

---

## Design considerations

- IDs are generated at the domain level
- Stock changes are performed exclusively through inventory movements
- Logical deletion is implemented using a `Status` field
- Inactive products are excluded from queries
- Each write operation has its own Command and Handler

---

## Stop the application

```bash
docker compose down -v
```
