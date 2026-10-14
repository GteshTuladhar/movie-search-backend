# Movie Search Backend Service

A production-grade, clean-architecture backend service built with **.NET 10** that integrates with The Movie Database (TMDB) API to provide trending movies, pagination searches, detailed movie information, and YouTube trailer video links.

---

## Architectural Overview

The solution follows strict **Clean Architecture** principles, partitioned into distinct layers to maintain separation of concerns, high testability, and maintainability:

- **`MovieSearchCase.Domain`**: Core enterprise business logic, domain models, custom domain exceptions, and interfaces (`ITmdbClient`). Free from external framework dependencies.
- **`MovieSearchCase.Infrastructure`**: External concerns, including the HTTP client implementation (`TmdbClient`) utilizing `HttpClientFactory`, DTO mappers, and upstream failure handling.
- **`MovieSearchCase.Shared`**: Common utilities, shared contracts, and extension helpers used across layers.
- **`MovieSearchCase.WebApi`**: Presentation layer featuring versioned REST endpoints (`v1`), request handlers factory pattern, input validation, robust ProblemDetails error responses, and Swagger/OpenAPI documentation.
- **`MovieSearchCase.Infrastructure.Tests`**: Unit and integration test suites validating downstream resilience and client mapping logic.

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed on your machine.
- A valid TMDB API Read Access Token or API Key.

### Configuration
1. Navigate to the `src/MovieSearchCase.WebApi` directory.
2. Update or create an `appsettings.json` (or use user-secrets) to provide your TMDB API configuration:
   ```json
   {
     "TmdbOptions": {
       "BaseUrl": "https://api.themoviedb.org/3/",
       "ApiKey": "YOUR_TMDB_API_KEY_HERE"
     }
   }
   ```

### Building and Running via CLI

1. **Restore dependencies and build the solution:**
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Run the API service:**
   ```bash
   cd src/MovieSearchCase.WebApi
   dotnet run
   ```
   The API will start locally (typically on `https://localhost:5001` or `http://localhost:5000`).

3. **Explore Swagger UI:**
   Navigate to `https://localhost:5001/swagger` in your browser to interact with the endpoints.

---

## Running Tests

To execute the automated unit and integration tests across the solution, run:
```bash
dotnet test
```

---

## API Endpoints (v1)

- **GET `/api/v1/movies/trending`** – Retrieves this week's trending movies from TMDB.
- **GET `/api/v1/movies/search?query={query}&page={page}`** – Searches movies by query string with pagination (pages 1–500 supported).
- **GET `/api/v1/movies/{id}`** – Fetches comprehensive details for a specific movie ID.
- **GET `/api/v1/movies/{id}/videos`** – Retrieves a filtered list of YouTube trailer videos associated with the movie.