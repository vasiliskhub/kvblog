# kvblog

Kvblog is a simple blog platform built with ASP.NET Core. The project
contains a REST API, a Razor Pages front end and a Postgres database. It
serves as a minimal example of how to build and deploy a small blog with
docker compose.

It was built for the needs of my minimal dev blog 

https://aiandchill.co.uk/

## Architecture

```
[PostgreSQL] <-EF Core-> [kvblog.api] <-HTTP-> [kvblog.client.razor]
```

- **kvblog.api** – ASP.NET Core Web API exposing CRUD endpoints for blog
  articles.
- **kvblog.api.db** – Entity Framework Core project with migrations and
  repository implementations.
- **kvblog.client.razor** – Razor Pages application that consumes the API
  and renders the blog UI.
- **docker-compose** files – compose the database, API and client
  containers for local development and production.

The backend and frontend images are defined by their respective
`Dockerfile` files and can be run together with Docker Compose.

## CI/CD

The repository includes a GitHub Actions workflow
(`.github/workflows/kvblogcicd.yml`). On every push to `main`, the
pipeline:

1. checks out the code and sets up .NET
2. runs `dotnet test` on the solution
3. builds and pushes the API and client Docker images to Docker Hub
4. uploads the repository to the Hetzner server via SCP
5. creates an `.env` file containing the Auth0 secrets on the server
6. redeploys the containers using `docker compose` on the remote host

## Running locally with Docker

To start a local development environment run:

```bash
docker compose -f docker-compose.dev.yml up --build
```

This will launch PostgreSQL, the API on port `8080` and the client on
port `8081`. Browse to <http://localhost:8081> to view the site. The dev
compose file already contains the required connection string for the API
container.

## appsettings / env variables

Use `appsettings.Development.json` for local development. Create it and
remember to always keep it local or just add env variables on your machine
(quite easy to be AI generated).

## Auth0 configuration

Authentication is handled by Auth0. Both the API and the client expect
several Auth0 settings which are supplied via environment variables in
production (`docker-compose.prod.yml`) and can be placed 
in `appsettings.Development.json` or env variables for local development.

You will need to setup your own Auth0 tenant at https://auth0.com/.

The API defines an authorization policy named `CUDAccess` that requires
a JWT claim `permissions` containing the value `kvblog:cud` (see
`kvblog.api/Program.cs`). The client reuses the same policy and also
checks that the user has the custom role `admin` stored in the
`https://apokapa.eu/roles` claim. To replicate this setup in Auth0 you
need:

1. **An API** entry (the audience used by both applications).
2. **A permission** called `kvblog:cud` added to that API.
3. **A role** (e.g. `admin`) that includes the `kvblog:cud` permission.
4. An Auth0 Action or Rule that adds the user's roles to the ID token in
a custom claim named `https://apokapa.eu/roles`. Feel free to rename permissions,
claims, roles etc and make the adjustments in code.
6. Two applications: a regular Web App for the Razor client and a
   machine‑to‑machine or SPA app for the API depending on your setup.

Assign the `admin` role to users who should be able to create, update or
delete articles. When they sign in, their access token will contain the
`permissions` claim and the ID token will include the custom role claim
used by the client.

