# Improvements — data_structure_dictionary

**Goal:** A deployed student-enrolment manager: a web frontend over a REST API with a real database, containerized and shipped via CI/CD — evolving the in-memory Dictionary demo into a full-stack cloud app.

## TL;DR — Path to production
- [x] Extract domain from WinForms into a cross-platform .NET 8/9 library + xUnit tests
- [x] ASP.NET Core Minimal API + Swagger
- [x] EF Core + PostgreSQL persistence (SQLite default; Postgres via connection string)
- [x] Web frontend for list/add/edit/delete/search (static HTML + fetch-based JS, served from wwwroot)
- [x] Docker + GitHub Actions CI/CD
- [ ] Live cloud demo with health checks + OpenTelemetry

## Current state
- .NET 6 **Windows Forms** desktop app (not a console app), Windows-only.
- Manages students (ID, name, enrol status) in an in-memory `Dictionary<string, Student>`; supports add/edit/delete/search/status.
- No persistence — all data lost on close. No tests, no CI, no packaging.
- Single-file logic in `Form1.cs`; domain model and UI tightly coupled.

## Key improvements
- **Backend:** replace WinForms with an ASP.NET Core Minimal API (or FastEndpoints); expose CRUD + search over students.
- **Persistence:** back it with EF Core + PostgreSQL; keep the Dictionary as an in-memory cache/repository abstraction.
- **Frontend:** a Blazor WebAssembly (or React + TypeScript) SPA for the student list, enrol status, and search.
- **Quality:** xUnit + FluentAssertions unit tests, integration tests via WebApplicationFactory/Testcontainers.
- **Delivery:** Dockerize API + DB with docker-compose; GitHub Actions CI (build/test/lint) and deploy.
- **Cross-platform:** drop `net6.0-windows`; target current .NET so it runs on Linux containers.

## Latest tech to showcase
- **.NET 8/9** Minimal APIs, EF Core, and native AOT-friendly containers.
- **Blazor WebAssembly** or **React 18 + Vite + TypeScript** frontend.
- **PostgreSQL** + **Testcontainers** for realistic integration tests.
- **GitHub Actions** CI/CD, container registry (GHCR), deploy to Azure Container Apps / Fly.io.
- **OpenTelemetry** + Swagger/OpenAPI for observability and API docs.

## Roadmap
1. Extract domain + repository, wrap in ASP.NET Core API with Swagger; add xUnit tests.
2. Add EF Core + PostgreSQL and a Blazor/React frontend; Dockerize with compose.
3. GitHub Actions CI/CD to a live cloud host with OpenTelemetry + health checks.
