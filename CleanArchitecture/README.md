# Clean Architecture - Address Service

## Database Configuration

### Connection Details
- **Server:** `(localdb)\ProjectModels`
- **Database:** `AddressDb`
- **Authentication:** Windows Authentication (Trusted Connection)

### Connection String
```
Server=(localdb)\ProjectModels;Database=AddressDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

## Database Migrations

### Add a New Migration
```bash
dotnet ef migrations add <MigrationName> --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Api
```

### Update Database
```bash
dotnet ef database update --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Api
```

### Remove Last Migration
```bash
dotnet ef migrations remove --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Api
```

### View Migration Script
```bash
dotnet ef migrations script --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Api
```

## Database Schema

### Addresses Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PRIMARY KEY, IDENTITY |
| Street | nvarchar(200) | NOT NULL |
| City | nvarchar(100) | NOT NULL |
| State | nvarchar(100) | NOT NULL |
| ZipCode | nvarchar(20) | NOT NULL |
| Country | nvarchar(100) | NOT NULL |
| CreatedAt | datetime2 | NOT NULL |
| UpdatedAt | datetime2 | NULL |

## API Documentation

### Swagger UI
The API includes interactive Swagger documentation:
- **URL:** `https://localhost:<port>/` (when running in Development mode)
- **Swagger JSON:** `https://localhost:<port>/swagger/v1/swagger.json`

Swagger provides:
- Interactive API testing interface
- Complete API documentation with XML comments
- Request/Response schemas
- Example values for all DTOs

## API Endpoints

### Get All Addresses
- **GET** `/api/addresses`
- **Response:** 200 OK with list of addresses

### Get Address by ID
- **GET** `/api/addresses/{id}`
- **Response:** 200 OK or 404 Not Found

### Create Address
- **POST** `/api/addresses`
- **Body:** CreateAddressDto
- **Response:** 201 Created or 400 Bad Request

### Update Address
- **PUT** `/api/addresses/{id}`
- **Body:** UpdateAddressDto
- **Response:** 200 OK, 400 Bad Request, or 404 Not Found

### Delete Address
- **DELETE** `/api/addresses/{id}`
- **Response:** 204 No Content or 404 Not Found

## Running the Application

1. Ensure SQL Server LocalDB is running
2. The database will be created automatically on first run if it doesn't exist
3. Run the application:
   ```bash
   dotnet run --project CleanArchitecture.Api
   ```
4. Open your browser and navigate to the URL shown in the console (typically `https://localhost:5001` or similar)
5. The Swagger UI will load automatically at the root URL

## Features

- ? Clean Architecture with proper layer separation
- ? Result pattern for explicit error handling
- ? Entity Framework Core with SQL Server
- ? Database migrations support
- ? Swagger/OpenAPI documentation
- ? XML comments for enhanced API documentation
- ? CRUD operations for Address entity
- ? Proper HTTP status codes and responses
