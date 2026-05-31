# Travel Explorer API (backend)

ASP.NET Core 8 Web API for the Travel Explorer platform: destinations, activities,
categories, flights, bookings, reviews, blogs, payments, contact messages, user
profiles, and admin management. Built with Clean Architecture + CQRS.

> Full endpoint reference: see [../API.md](../API.md).
> Whole-system overview and setup: see the [root README](../README.md).

## Tech stack

- .NET 8 / ASP.NET Core Web API, C#
- Entity Framework Core 8 + PostgreSQL (Npgsql), `pg_trgm` fuzzy search
- MediatR (CQRS), AutoMapper, FluentValidation
- ASP.NET Core Identity + JWT bearer auth, Google OAuth (OIDC)
- Cloudinary (image upload), Paymob (payments), Polly (HTTP resilience)
- Swagger / Swashbuckle

## Solution structure

The solution `Travel Explorer.sln` contains four projects (Clean Architecture):

```
Travel Explorer/                 # API layer: Controllers, Middleware, Program.cs, config
Travel Explorer.Application/     # CQRS features (Commands/Queries), DTOs, validators, mapping, behaviors
Travel Explorer.Domain/          # Entities, enums, base classes, repository interfaces (no deps)
Travel Explorer.Infrastructure/  # EF Core DbContext, configurations, migrations, repositories, services
```

Dependency direction: `API -> Application -> Domain <- Infrastructure`.

Key patterns: CQRS via MediatR (one Command/Query + Handler per operation), the
Specification pattern for queries, Repository + Unit of Work, and MediatR pipeline
behaviors (`ValidationBehavior`, `UserBlockBehavior`). Soft-delete is enforced with
EF global query filters.

## Configuration

The API reads config from User Secrets, `appsettings*.json`, and environment variables.
See [.env.example](.env.example) for every key and how to set it.

- `appsettings.Development.json` already ships with safe **local** defaults (a Postgres
  connection string, a dev JWT signing key, and Google redirect URLs pointing at the
  SPA at `http://localhost:5173`). Change the connection string to match your database.
- Secrets for non-local use (real JWT key, Google, Cloudinary, Paymob) should go in
  User Secrets or environment variables, never in source control.

Minimum to run locally: a reachable PostgreSQL `ConnectionStrings:DefaultConnection`
(JWT dev defaults are already present).

## Run

```bash
# from the repo root
dotnet restore "Travel Explorer.sln"
dotnet run --project "Travel Explorer" --launch-profile https
```

- HTTPS: `https://localhost:7133`  |  HTTP: `http://localhost:5015`
- Swagger UI: `https://localhost:7133/swagger`
- Trust the dev certificate once: `dotnet dev-certs https --trust`

On startup the app automatically applies EF Core migrations and seeds the roles plus a
default admin account (see below). On an empty database it also seeds realistic demo content
(categories, destinations with real images, activities, flights, blog posts, and reviews) plus a
few demo users, so the app looks alive out of the box. Seeding is idempotent—it only runs when no
destinations exist yet.

## Database / migrations

- PostgreSQL. Migrations live in `Travel Explorer.Infrastructure/Migrations` and are
  applied automatically at startup (`db.Database.MigrateAsync()` in `Program.cs`).
- To create the database manually or add migrations you need the EF tool:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add <Name> --project "Travel Explorer.Infrastructure" --startup-project "Travel Explorer"
dotnet ef database update      --project "Travel Explorer.Infrastructure" --startup-project "Travel Explorer"
```

## Authentication & roles

- JWT bearer. `POST /api/Auth/login` returns `{ token: { accessToken, refreshToken } }`.
- Access tokens are short-lived; use `POST /api/Auth/refresh-token` with the refresh
  token to rotate. `POST /api/Auth/logout` revokes a refresh token.
- Authorization is driven by the role claim in the JWT. Roles: `Admin`, `Traveler`,
  `Author`.
- A default admin is seeded on first run (configurable via `AdminUser:*`):
  `admin` / `Admin@123`.
- Demo users are also seeded on first run: an author (`maya.lawson`) and several travelers
  (e.g. `liam.carter`, `ava.rossi`), all with the password `Password@123`.
- Google OAuth endpoints (`/api/Auth/google-login`, `/api/Auth/google-register`) require
  `JwtSettings:GoogleClientId/Secret`; the callback redirects to the SPA URLs in config. The Google
  authentication scheme is only registered when those credentials are present, so the rest of the
  API runs fine without them (the endpoints stay dormant until configured).

## Error format

A global exception middleware returns JSON problem details and maps exceptions to status
codes: `NotFoundException`->404, `BadRequestException`->400, `ConflictException`->409,
`ForbiddenAccessException`->403, `UnauthorizedAccessException`->401, validation->400,
anything else->500. Stack traces are only included in Development.

```json
{
  "Status": 400,
  "Type": "ValidationFailure",
  "Title": "One or more validation errors occurred.",
  "Instance": "/api/Destinations",
  "Errors": { "Name": ["Destination name is required."] }
}
```

## Troubleshooting

- "Failed to connect to ...:5432": PostgreSQL isn't running or the connection string is
  wrong. Fix `ConnectionStrings:DefaultConnection`.
- HTTPS/cert warnings from the client: run `dotnet dev-certs https --trust`, or point the
  client proxy at the HTTP URL via `VITE_DEV_API_TARGET`.
- `JwtSettings configuration section is missing`: provide `JwtSettings:*` (present by
  default in `appsettings.Development.json`).
- Google / Cloudinary / Paymob features do nothing until their credentials are set.
