# InventoryManagement Microservice

InventoryManagement is a small ASP.NET Core 8 microservice that keeps track of products, categories, and stock movements. It exposes RESTful APIs for CRUD operations and stock adjustments, and ships with an accompanying xUnit test suite that covers business logic, service behavior, and controller status codes.

## Project Layout

```
Inventory-management-microservice/
├── InventoryManagment/          # Main ASP.NET Core application
│   ├── Controllers/             # Products, Categories, Stock APIs
│   ├── Data/                    # EF Core DbContext
│   ├── Data.Models/             # Entity models
│   ├── DTOs/                    # Request/response contracts
│   └── Services/                # Business and application services
└── InventoryManagmentTest/      # xUnit tests (business, service, controllers)
```

## Tech Stack
- **Framework:** ASP.NET Core 8 (Web API)
- **Data Access:** Entity Framework Core (SQL Server by default, SQLite package available)
- **Object Mapping:** AutoMapper
- **API Docs:** Swagger / Swashbuckle
- **Testing:** xUnit, Moq, FluentAssertions, EF Core InMemory provider

## Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- SQL Server instance (local or remote). The default connection string targets `Server=.;Database=InventoryManagmentDb;` and trusts the local machine certificate.
- Optional: Docker (if you want to use the provided Dockerfile)

## Getting Started

1. **Restore dependencies**
   ```bash
   dotnet restore InventoryManagment.sln
   ```

2. **Configure the database**
   - Update `InventoryManagment/appsettings.json` (or `appsettings.Development.json`) with your SQL Server connection string if needed.
   - Apply migrations (creates the database if it does not exist):
     ```bash
     dotnet ef database update --project InventoryManagment
     ```

3. **Run the API**
   ```bash
   dotnet run --project InventoryManagment
   ```
   The API hosts Swagger UI at `https://localhost:7193/swagger` by default (see the console output for the exact port).

## Key API Endpoints
| HTTP Verb | Route                               | Description                            |
|-----------|-------------------------------------|----------------------------------------|
| GET       | `/api/products`                     | List all products                      |
| GET       | `/api/products/{id}`                | Retrieve a single product              |
| POST      | `/api/products`                     | Create a product                       |
| PUT       | `/api/products/{id}`                | Update product details (no stock)      |
| DELETE    | `/api/products/{id}`                | Remove a product                       |
| GET       | `/api/categories`                   | List all categories                    |
| POST      | `/api/categories`                   | Create a category                      |
| GET       | `/api/categories/with-products`     | Categories with their products         |
| POST      | `/api/products/{id}/stock/increase` | Increase stock for a product           |
| POST      | `/api/products/{id}/stock/decrease` | Decrease stock (validates availability)|
| GET       | `/api/products/{id}/stock/transactions` | Stock transaction history          |

> **Note:** Product quantity changes are handled exclusively through the stock endpoints, and each change automatically creates a `StockTransaction` record.

## Running Tests
The solution includes a layered test suite:
- **Tier 1 (Business Logic):** InventoryService stock adjustments and transaction history
- **Tier 2 (Services):** ProductService and CategoryService behaviors and validation
- **Tier 3 (Controllers):** HTTP status code verification for key endpoints

Run all tests with:
```bash
dotnet test InventoryManagmentTest
```

## Docker Support
A `Dockerfile` is available under `InventoryManagment/`. Build and run it with:
```bash
docker build -t inventory-management ./InventoryManagment
# Update connection string via environment variables before running the container
```

Example run command with a connection string override:
```bash
docker run -e ConnectionStrings__DefaultConnection="Server=.;Database=InventoryManagmentDb;Trusted_Connection=True;" -p 8080:8080 inventory-management
```

## Why this project is useful
This project demonstrates CRUD endpoints, service layering, DTO-based contracts, stock tracking with history, automated tests, and Docker packaging—all achievable with a focus on simplicity and clarity.

## Troubleshooting
- **Migrations fail / database not reachable:** Verify SQL Server is running and the connection string matches your environment.
- **Port already in use:** Override Kestrel settings via `launchSettings.json` or environment variables (e.g., `ASPNETCORE_URLS`).
- **Swagger not loading:** Ensure HTTPS developer certificates are trusted (`dotnet dev-certs https --trust`).

## Contributing
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/xyz`)
3. Keep changes focused and easy to understand
4. Add or update tests where necessary
5. Open a pull request describing your changes

Enjoy building on InventoryManagement!