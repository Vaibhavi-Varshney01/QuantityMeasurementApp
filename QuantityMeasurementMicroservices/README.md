# QMA Microservice Architecture

## Overview

This project converts the original 4-tier N-Tier QMA application into a **5-service microservice architecture**.

```
Client
  │
  ▼
┌─────────────────────────────────────────────────────┐
│             api-gateway  :5000                      │
│  • Routes all requests                              │
│  • Validates JWT (guards /quantities endpoints)     │
│  • Swagger UI for the whole system                  │
└──────────┬─────────────────────┬────────────────────┘
           │                     │
           ▼                     ▼
┌──────────────────┐   ┌──────────────────────────────┐
│  auth-service    │   │       qma-service  :5002      │
│  :5001           │   │  • Business logic             │
│  • Register      │   │  • Compare / Add / Subtract   │
│  • Login         │   │  • Divide / Convert           │
│  • JWT issue     │   │  • History queries            │
│  • BCrypt hash   │   └──────────┬───────────────────┘
└──────────────────┘              │
           │                      ▼
           │          ┌──────────────────────────────┐
           │          │   repository-service  :5003   │
           │          │  • EF Core (SQL Server)       │
           │          │  • Redis cache layer          │
           │          │  • Internal REST API          │
           │          └──────────────────────────────┘
           │                      │
           └──────────────────────┘
                    Shared DB (SQL Server)
                    Cache     (Redis)
```

## N-Tier to Microservice Mapping

| N-Tier Layer                    | Microservice         | Port |
|---------------------------------|----------------------|------|
| Presentation (WebAPI/Controllers) | api-gateway        | 5000 |
| Auth (Business sub-concern)     | auth-service         | 5001 |
| Business Layer                  | qma-service          | 5002 |
| Repository Layer (Data Access)  | repository-service   | 5003 |
| Model Layer                     | shared-models (lib)  | —    |
| Redis Cache (infra)             | redis (Docker)       | 6379 |

## Running with Docker Compose

```bash
docker-compose up --build
```

Then open: http://localhost:5000/swagger

## Running Locally (without Docker)

Start each service in order:

```bash
# 1. repository-service
cd repository-service && dotnet run

# 2. auth-service
cd auth-service && dotnet run

# 3. qma-service
cd qma-service && dotnet run

# 4. api-gateway
cd api-gateway && dotnet run
```

## API Usage

### Step 1 — Register
```
POST http://localhost:5000/api/v1/auth/register
{ "username": "alice", "password": "Secret123!" }
```

### Step 2 — Login (get token)
```
POST http://localhost:5000/api/v1/auth/login
{ "username": "alice", "password": "Secret123!" }
```

### Step 3 — Use token for quantity operations
```
Authorization: Bearer <token>

POST http://localhost:5000/api/v1/quantities/compare
{
  "thisQuantityDTO":  { "value": 1, "unit": "feet", "measurementType": "length" },
  "thatQuantityDTO":  { "value": 12, "unit": "inch", "measurementType": "length" }
}
```

## Service-to-Service Communication

- `api-gateway` → `auth-service`   : HTTP (proxies auth requests)
- `api-gateway` → `qma-service`    : HTTP (proxies quantity requests + forwards JWT)
- `qma-service` → `repository-service` : HTTP (internal only, not exposed to clients)

## Configuration

All service URLs and JWT settings live in each service's `appsettings.json`.
When using Docker Compose, environment variables override these automatically.
