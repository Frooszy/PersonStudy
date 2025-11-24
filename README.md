# About

This is a CRUD project for study, carried out in .NET 9, using Entity Framework Core, PostgreSQL and docker.

## Requirements

Make sure you have:

- .NET 9 SDK
- Docker

## How to Run

1. Make sure Docker is installed and runnning.
2. Apply database migrations:

```bash
dotnet ef database update
```

3. Start the application using Docker Compose:
```bash
docker compose up
```

The API will be available at http://localhost:5000.

### Endpoints

This project uses Swagger to document the API endpoints. You can access it in your browser at: http://localhost:5000/swagger.