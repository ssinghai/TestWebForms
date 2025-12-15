# ASP.NET Web Forms to .NET 8.0 Razor Pages Migration Guide

## Overview

This document describes the migration of the WingtipToys application from ASP.NET Web Forms (.NET Framework 4.5.2) to ASP.NET Core Razor Pages (.NET 8.0).

## Architecture Changes

### Project Structure

**Before (Web Forms):**
```
WingtipToys/
├── WingtipToys.csproj (old-style csproj)
├── Web.config
├── Global.asax
├── Site.Master (master page)
├── *.aspx (Web Forms pages)
├── *.aspx.cs (code-behind files)
├── Models/
├── Logic/
└── App_Start/
```

**After (Razor Pages):**
```
WingtipToysCore/
├── WingtipToysCore.csproj (SDK-style)
├── Program.cs
├── appsettings.json
├── Pages/
│   ├── _Layout.cshtml (shared layout)
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   └── *.cshtml (Razor Pages)
├── Models/
├── Data/
├── Services/
└── wwwroot/ (static files)
```

## Key Technical Changes

### 1. Project File Format

**Old (.NET Framework):**
- XML-based project file with explicit file references
- Uses packages.config for NuGet
- Targets .NET Framework 4.5.2

**New (.NET 8.0):**
- SDK-style project format
- PackageReference for NuGet
- Implicit file inclusion
- Nullable reference types enabled
- Target framework: net8.0

### 2. Dependency Injection

**Before:**
```csharp
// Direct instantiation in code-behind
var _db = new ProductContext();
var cart = new ShoppingCartActions();
```

**After:**
```csharp
// Constructor injection
public class ProductListModel : PageModel
{
    private readonly ProductContext _context;
    
    public ProductListModel(ProductContext context)
    {
        _context = context;
    }
}
```

### 3. Entity Framework

**Before:** Entity Framework 6 (EF6)
- DbContext with parameterless constructor
- Database.SetInitializer for seeding
- Connection string from Web.config

**After:** Entity Framework Core 8
- DbContext with DbContextOptions
- Database initialization in Program.cs
- Connection string from appsettings.json
- Async operations supported

### 4. Configuration

**Before (Web.config):**
```xml
<connectionStrings>
    <add name="WingtipToys" 
         connectionString="Data Source=..." />
</connectionStrings>
```

**After (appsettings.json):**
```json
{
  "ConnectionStrings": {
    "WingtipToys": "Data Source=..."
  }
}
```

### 5. Session State

**Before:**
```csharp
HttpContext.Current.Session["CartId"]
```

**After:**
```csharp
// Requires session middleware configuration
HttpContext.Session.SetString("CartId", value);
HttpContext.Session.GetString("CartId");
```

### 6. Page Model

**Before (Web Forms):**
```csharp
public partial class ProductList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { }
    
    public IQueryable<Product> GetProducts([QueryString("id")] int? id)
    {
        // Data binding method
    }
}
```

**After (Razor Pages):**
```csharp
public class ProductListModel : PageModel
{
    public List<Product> Products { get; set; }
    
    public void OnGet(int? id)
    {
        Products = _context.Products.ToList();
    }
}
```

## Page Migration Details

### 1. Default.aspx → Index.cshtml
- Simple content page
- Master page → _Layout.cshtml
- ContentPlaceHolder → @RenderBody()

### 2. About.aspx → About.cshtml
- Static content page
- No business logic

### 3. Contact.aspx → Contact.cshtml
- Static content page
- Contact information display

### 4. ProductList.aspx → ProductList.cshtml
- Data-driven page
- ListView control → Razor foreach loop
- Model binding → Direct property access
- Query string parameter handling updated

### 5. ProductDetails.aspx → ProductDetails.cshtml
- FormView control → Conditional rendering
- Data binding expressions updated
- Query string parameter handling

### 6. ShoppingCart.aspx → ShoppingCart.cshtml
- GridView → HTML table with form
- ViewState → Model binding
- Server-side button click → OnPost handler
- UpdatePanel removed (no longer needed)

### 7. AddToCart.aspx → AddToCart.cshtml
- Redirect-only page
- Query string handling
- Immediate redirect to shopping cart

## Breaking Changes

### 1. ViewState and PostBack
- **Web Forms:** Heavy use of ViewState for state management
- **Razor Pages:** No ViewState; use model binding and hidden fields

### 2. Server Controls
- **Web Forms:** Rich server controls (GridView, FormView, ListView)
- **Razor Pages:** Standard HTML with Razor syntax

### 3. Page Lifecycle
- **Web Forms:** Complex page lifecycle (Init, Load, PreRender, etc.)
- **Razor Pages:** Simplified OnGet/OnPost handlers

### 4. Master Pages
- **Web Forms:** Master pages with ContentPlaceHolders
- **Razor Pages:** _Layout.cshtml with @RenderBody()

### 5. Global Application Events
- **Web Forms:** Global.asax with Application_Start
- **Razor Pages:** Program.cs with startup configuration

### 6. Routing
- **Web Forms:** Physical file paths or friendly URLs
- **Razor Pages:** Convention-based routing

## Services and Middleware

The new application uses the following services:

```csharp
// Entity Framework Core
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(connectionString));

// Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Custom services
builder.Services.AddScoped<ShoppingCartService>();
```

## Running the Application

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server or SQL Server LocalDB

### Build and Run
```bash
cd WingtipToys/WingtipToysCore
dotnet restore
dotnet build
dotnet run
```

The application will be available at `https://localhost:5001` (or the port shown in the console).

### Database
The database will be automatically created and seeded on first run using the `DbInitializer` class.

## Modernization Benefits

1. **Performance:** ASP.NET Core is significantly faster than Web Forms
2. **Cross-platform:** Can run on Windows, Linux, and macOS
3. **Modern tooling:** Better IDE support and debugging
4. **Dependency injection:** Built-in DI container
5. **Middleware pipeline:** Flexible request processing
6. **Async/await:** First-class async support throughout
7. **Cloud-ready:** Better suited for containerization and cloud deployment
8. **Active development:** Regular updates and LTS releases
9. **Open source:** Full framework is open source
10. **Better testability:** Easier to unit test with DI

## Known Limitations

1. **No mobile master page:** The Site.Mobile.Master has not been migrated (mobile-first design recommended)
2. **No ViewSwitcher:** Mobile/desktop switching not implemented (use responsive design instead)
3. **No bundling/minification:** The Web Forms bundling has been replaced with standard approaches
4. **Simplified validation:** Client-side validation uses different approach

## Next Steps for Further Modernization

1. **Add authentication:** Implement ASP.NET Core Identity
2. **API endpoints:** Add Web API controllers for AJAX operations
3. **Modern JavaScript:** Replace jQuery with modern frameworks (React, Vue, etc.)
4. **Responsive design:** Update CSS to be mobile-first
5. **Async operations:** Convert synchronous DB operations to async
6. **Caching:** Implement response caching and distributed caching
7. **Logging:** Add structured logging (Serilog, NLog)
8. **Health checks:** Add health check endpoints
9. **Docker:** Containerize the application
10. **CI/CD:** Set up automated build and deployment

## Troubleshooting

### Database Connection Issues
If you encounter database connection issues:
1. Ensure SQL Server LocalDB is installed
2. Update the connection string in `appsettings.json`
3. Check that the database is created (will auto-create on first run)

### Port Conflicts
If the default port is in use:
1. Check `Properties/launchSettings.json`
2. Update the port numbers as needed

### Static Files Not Loading
Ensure `app.UseStaticFiles()` is called in `Program.cs` before `app.UseRouting()`.

## Migration Checklist

- [x] Create new .NET 8.0 project
- [x] Migrate data models
- [x] Migrate Entity Framework to EF Core
- [x] Convert Web Forms pages to Razor Pages
- [x] Migrate static assets
- [x] Update configuration
- [x] Migrate business logic
- [x] Test all functionality
- [x] Update documentation

## Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Razor Pages Documentation](https://docs.microsoft.com/aspnet/core/razor-pages)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [Migration from ASP.NET to ASP.NET Core](https://docs.microsoft.com/aspnet/core/migration)
