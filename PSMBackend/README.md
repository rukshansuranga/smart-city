# PSMBackend

## Overview

PSMBackend is a .NET Core Web API project for managing clients, users, tickets, work packages, and related entities. It uses Entity Framework Core with PostgreSQL and follows a layered architecture with DataAccess, Model, and WebAPI projects.

## Project Structure

- **CodingWiki_DataAccess/**: Data access layer (Entity Framework Core)
- **DataAccess/**: Contains DbContext, entity configurations, and migrations
- **Model/**: Entity models and enums
- **WebAPI/**: ASP.NET Core Web API controllers, DTOs, services, and utilities

## Getting Started

### Prerequisites

- .NET 6 SDK or later
- PostgreSQL database

### Setup

1. Clone the repository.
2. Update the connection string in `ApplicationDbContext.cs` or `appsettings.json` as needed.
3. Restore NuGet packages:
   ```sh
   dotnet restore
   ```
4. Create migrations and update the database:
   ```sh
   dotnet ef migrations add -p .\DataAccess\PSMDataAccess.csproj -s .\WebAPI\PSMWebAPI.csproj init
   dotnet ef database  update -p .\DataAccess\PSMDataAccess.csproj -s .\WebAPI\PSMWebAPI.csproj
   ```
5. Run the API:
   ```sh
   dotnet run --project WebAPI/PSMWebAPI.csproj
   ```

## API Endpoints

- `api/Client` - CRUD operations for clients
- Additional endpoints for users, tickets, work packages, etc.

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- AutoMapper

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](LICENSE)
