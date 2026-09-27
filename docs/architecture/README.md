# BookItNepal Architecture

## Project Layers

- `BookIT.Domain` is the topmost layer. It owns entities and enums and must not reference other project libraries.
- `BookIT.Application` owns DTOs, abstractions, and services. It may reference `BookIT.Domain`.
- `BookIT.Persistence` owns the EF Core DbContext, migrations, entity configurations, and repository implementations. It references `BookIT.Domain` and `BookIT.Application`.
- `BookIT.Infrastructure` owns infrastructure implementations such as token and password services, plus integrations with external systems such as email and AWS.

## Database

- PostgreSQL is the database.
- Use EF Core migrations for schema changes.
- Use snake_case naming for database objects.
