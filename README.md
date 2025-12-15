# WingtipToys - Migrated to .NET 8.0

This repository contains the WingtipToys sample e-commerce application, which has been migrated from ASP.NET Web Forms (.NET Framework 4.5.2) to ASP.NET Core Razor Pages (.NET 8.0).

## Projects

### WingtipToys (Legacy - .NET Framework 4.5.2)
The original ASP.NET Web Forms application. This project is preserved for reference and comparison purposes.

**Technology Stack:**
- ASP.NET Web Forms
- .NET Framework 4.5.2
- Entity Framework 6
- Bootstrap 3
- jQuery

### WingtipToysCore (Modernized - .NET 8.0) ⭐
The modernized version using ASP.NET Core Razor Pages.

**Technology Stack:**
- ASP.NET Core 8.0
- Razor Pages
- Entity Framework Core 8.0
- Bootstrap 5
- jQuery
- SQL Server / LocalDB

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- SQL Server 2019 or later, or SQL Server LocalDB
- Visual Studio 2022 or VS Code (optional)

### Running the Modernized Application

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ssinghai/TestWebForms.git
   cd TestWebForms/WingtipToys/WingtipToysCore
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the project:**
   ```bash
   dotnet build
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

5. **Access the application:**
   - Open your browser and navigate to `https://localhost:5001` or the URL shown in the console
   - The database will be automatically created and seeded on first run

## Application Features

- **Product Catalog:** Browse toys by category (Cars, Planes, Trucks, Boats, Rockets)
- **Product Details:** View detailed information about each product
- **Shopping Cart:** Add items to cart, update quantities, remove items
- **Session Management:** Cart persists across page loads using sessions
- **Category Navigation:** Dynamic category menu from database

## Project Structure

```
WingtipToysCore/
├── Program.cs                 # Application entry point and configuration
├── appsettings.json          # Configuration settings
├── Pages/                    # Razor Pages
│   ├── Index.cshtml         # Home page
│   ├── ProductList.cshtml   # Product listing
│   ├── ProductDetails.cshtml # Product details
│   ├── ShoppingCart.cshtml  # Shopping cart
│   ├── AddToCart.cshtml     # Add to cart handler
│   ├── About.cshtml         # About page
│   ├── Contact.cshtml       # Contact page
│   └── Shared/
│       └── _Layout.cshtml   # Shared layout
├── Models/                   # Data models
│   ├── Product.cs
│   ├── Category.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   └── OrderDetail.cs
├── Data/                     # Data access
│   ├── ProductContext.cs    # EF Core DbContext
│   └── DbInitializer.cs     # Database seeding
├── Services/                 # Business logic
│   └── ShoppingCartService.cs
└── wwwroot/                  # Static files
    ├── Catalog/             # Product images
    ├── Images/              # Site images
    ├── Content/             # CSS files
    └── lib/                 # JavaScript libraries
```

## Migration Documentation

For detailed information about the migration from Web Forms to Razor Pages, including:
- Architecture changes
- Breaking changes
- Code comparisons
- Migration patterns
- Troubleshooting guide

See [MIGRATION.md](MIGRATION.md)

## Key Differences from Web Forms

| Aspect | Web Forms | Razor Pages |
|--------|-----------|-------------|
| **Page Model** | Code-behind with page lifecycle | PageModel with OnGet/OnPost handlers |
| **State Management** | ViewState, Session | Model binding, Session |
| **Server Controls** | Rich server controls | HTML with Razor syntax |
| **Dependency Injection** | Manual instantiation | Built-in DI container |
| **Routing** | Physical file paths | Convention-based routing |
| **Configuration** | Web.config | appsettings.json |
| **Data Access** | Entity Framework 6 | Entity Framework Core 8 |

## Configuration

### Database Connection

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "WingtipToys": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WingtipToys;Integrated Security=True"
  }
}
```

### Session Settings

Session timeout and cookie settings can be configured in `Program.cs`:

```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

## Development

### Building

```bash
dotnet build
```

### Running in Development Mode

```bash
dotnet run --environment Development
```

### Database Migrations

If you modify the models, create a migration:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Testing

The application can be tested manually by:

1. **Browse Products:** Navigate to Products page and filter by category
2. **View Details:** Click on a product to see details
3. **Add to Cart:** Add items to the shopping cart
4. **Update Cart:** Modify quantities or remove items
5. **Session Persistence:** Verify cart persists across page refreshes

## Deployment

### Publishing

To publish the application for deployment:

```bash
dotnet publish -c Release -o ./publish
```

### Docker (Optional)

A Dockerfile can be added for containerization:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WingtipToysCore.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WingtipToysCore.dll"]
```

## Contributing

This is a sample/demonstration project showing Web Forms to Razor Pages migration. Feel free to:

- Report issues
- Suggest improvements
- Submit pull requests
- Use as a reference for your own migrations

## License

This project is provided as-is for educational and demonstration purposes.

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Razor Pages Tutorial](https://docs.microsoft.com/aspnet/core/tutorials/razor-pages)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Migrating from ASP.NET to ASP.NET Core](https://docs.microsoft.com/aspnet/core/migration)

## Support

For questions or issues related to this migration:
1. Check the [MIGRATION.md](MIGRATION.md) documentation
2. Review the code comments
3. Open an issue on GitHub

---

**Original Web Forms Version:** Preserved in `WingtipToys/WingtipToys/` directory  
**Modernized Version:** Located in `WingtipToys/WingtipToysCore/` directory
