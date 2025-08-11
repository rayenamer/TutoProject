/// <summary>
/// THE APPLICATION STARTUP FILE - This is your application's entry point and configuration hub.
/// Think of this file as the "control center" that sets up EVERYTHING your app needs to run.
/// 
/// WHAT THIS FILE DOES:
/// - Creates and configures the web application host
/// - Registers all services for dependency injection
/// - Sets up the database connection
/// - Configures the HTTP request pipeline
/// - Starts the web server and begins listening for requests
/// 
/// WHY THIS MATTERS:
/// Without this file, your application would have no way to start, no services would be
/// registered, and no database connection would be established. This is the foundation
/// that brings all your components together into a working application.
/// 
/// STUDENT REALITY CHECK: This is NOT just a configuration file - it's the heart of your
/// application's startup process. Every ASP.NET Core app needs this file to run.
/// </summary>

using TutoProject.Data;
using Microsoft.EntityFrameworkCore;
using TutoProject.Repositories;

/// <summary>
/// THE BUILDER PATTERN - This is how modern ASP.NET Core applications are configured.
/// 
/// WHAT'S HAPPENING HERE:
/// - WebApplication.CreateBuilder(args) = "Create a builder for configuring services and middleware"
/// - var builder = ... = "Store the builder in a variable for further configuration"
/// - args = "Command-line arguments passed to the application"
/// 
/// WHY THIS PATTERN:
/// - Fluent API: Makes configuration code more readable and chainable
/// - Separation of Concerns: Clearly separates service registration from middleware configuration
/// - Testability: Makes it easier to create test hosts with custom configurations
/// 
/// STUDENT ALERT: The builder pattern is a modern replacement for the older Startup.cs approach.
/// It consolidates configuration into a single file for simplicity.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// THE SERVICE REGISTRATION PHASE - This is where you tell the dependency injection container
/// about all the services your application will need.
/// 
/// WHAT'S HAPPENING HERE:
/// - builder.Services = "The dependency injection container"
/// - AddEndpointsApiExplorer() = "Enable API endpoint discovery for Swagger"
/// - AddSwaggerGen() = "Generate OpenAPI documentation for your API"
/// 
/// WHY SWAGGER:
/// - API Documentation: Automatically generates interactive API documentation
/// - Testing: Provides a UI for testing your API endpoints without writing code
/// - Client Generation: Can generate client code for consuming your API
/// 
/// STUDENT REALITY CHECK: Swagger is incredibly useful during development, but you might
/// want to disable it in production for security and performance reasons.
/// </summary>
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// THE DATABASE CONFIGURATION - This is where you connect your application to the database.
/// 
/// WHAT'S HAPPENING HERE:
/// - AddDbContext<AppDbContext> = "Register your database context class for dependency injection"
/// - options => options.UseSqlite() = "Configure the context to use SQLite"
/// - "Data Source=products.db" = "The connection string pointing to your database file"
/// 
/// WHY SQLITE:
/// - Simplicity: SQLite is a file-based database that requires no separate server
/// - Portability: The database is just a file that can be easily copied or backed up
/// - Development: Perfect for development and learning scenarios
/// 
/// STUDENT ALERT: In a production application, you might want to use a more robust database
/// like SQL Server, PostgreSQL, or MySQL. You would only need to change the provider here.
/// 
/// REAL-WORLD EXAMPLE: To switch to SQL Server, you would change this to:
/// options.UseSqlServer("Server=myserver;Database=mydb;Trusted_Connection=True;");
/// </summary>
// Added database context with sqlite config (browse extensions and download it)
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=products.db"));

/// <summary>
/// THE REPOSITORY REGISTRATION - This is where you connect your interfaces to their implementations.
/// 
/// WHAT'S HAPPENING HERE:
/// - AddScoped<IProductRepository, ProductRepository> = "Register ProductRepository as the implementation of IProductRepository"
/// - AddScoped = "Create a new instance of the service for each HTTP request"
/// 
/// WHY SCOPED LIFETIME:
/// - AddSingleton = "One instance for the entire application lifetime"
/// - AddScoped = "One instance per HTTP request"
/// - AddTransient = "New instance every time it's requested"
/// - Scoped is perfect for repositories that use DbContext (which is also scoped)
/// 
/// STUDENT REALITY CHECK: The lifetime you choose is critical for proper resource management.
/// Using the wrong lifetime can lead to memory leaks or unexpected behavior.
/// 
/// DEPENDENCY INJECTION BENEFIT: Your controllers can now request IProductRepository
/// without knowing or caring about the concrete ProductRepository implementation.
/// </summary>
// register repositories so you can use them
builder.Services.AddScoped<IProductRepository, ProductRepository>();

/// <summary>
/// THE CONTROLLER REGISTRATION - This is where you enable MVC controllers in your application.
/// 
/// WHAT'S HAPPENING HERE:
/// - AddControllers() = "Enable controller-based routing for your API endpoints"
/// - This registers the MVC controller services without the view-related services
/// 
/// WHY JUST CONTROLLERS:
/// - AddControllers() = "API-only, no views or pages"
/// - AddControllersWithViews() = "API + MVC views"
/// - AddRazorPages() = "API + Razor Pages"
/// - For a pure API, AddControllers() is more efficient
/// 
/// STUDENT ALERT: This is what enables your ProductsController to handle HTTP requests.
/// Without this line, your controller endpoints would not be accessible.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// THE APPLICATION BUILDING PHASE - This is where the builder creates the actual application.
/// 
/// WHAT'S HAPPENING HERE:
/// - builder.Build() = "Finalize the service configuration and create the application"
/// - var app = ... = "Store the application in a variable for middleware configuration"
/// 
/// WHY SEPARATE BUILDING STEP:
/// - Services must be registered before the application is built
/// - After building, you can no longer register services
/// - This enforces a clear separation between service registration and middleware configuration
/// 
/// STUDENT REALITY CHECK: This line marks the transition from "registering what your app needs"
/// to "configuring how your app behaves" - two distinct phases of application startup.
/// </summary>
var app = builder.Build();

/// <summary>
/// THE CONTROLLER ROUTE MAPPING - This is where you connect HTTP requests to controller actions.
/// 
/// WHAT'S HAPPENING HERE:
/// - app.MapControllers() = "Discover and map all controller endpoints to HTTP routes"
/// - This automatically finds all your [Route] and [Http*] attributes
/// 
/// WHY EXPLICIT MAPPING:
/// - Makes the routing configuration visible in Program.cs
/// - Ensures controllers are properly discovered and mapped
/// - Required for controllers to receive HTTP requests
/// 
/// STUDENT ALERT: Without this line, your API endpoints would not be accessible,
/// even though your controllers are registered. Registration and mapping are separate steps.
/// </summary>
app.MapControllers(); // Map controllers to routes

/// <summary>
/// THE ENVIRONMENT-SPECIFIC CONFIGURATION - This is where you add features only for development.
/// 
/// WHAT'S HAPPENING HERE:
/// - if (app.Environment.IsDevelopment()) = "Only execute this code in development environment"
/// - app.UseSwagger() = "Enable the Swagger JSON endpoint"
/// - app.UseSwaggerUI() = "Enable the Swagger web UI for testing your API"
/// 
/// WHY CHECK ENVIRONMENT:
/// - Development tools should not be exposed in production
/// - Different environments need different configurations
/// - This pattern keeps your production app secure and optimized
/// 
/// STUDENT REALITY CHECK: The environment is determined by the ASPNETCORE_ENVIRONMENT
/// environment variable. Common values are "Development", "Staging", and "Production".
/// 
/// SECURITY NOTE: Never expose Swagger UI in production as it reveals details about your API
/// that could be useful to attackers.
/// </summary>
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// <summary>
/// THE HTTPS REDIRECTION MIDDLEWARE - This is where you enforce secure connections.
/// 
/// WHAT'S HAPPENING HERE:
/// - app.UseHttpsRedirection() = "Redirect HTTP requests to HTTPS"
/// - This ensures all communication with your API is encrypted
/// 
/// WHY HTTPS:
/// - Security: Encrypts all data between client and server
/// - Trust: Modern browsers warn users about non-HTTPS sites
/// - Features: Some browser features require HTTPS (geolocation, etc.)
/// 
/// STUDENT ALERT: For this to work in production, you need a valid SSL certificate.
/// In development, ASP.NET Core creates a self-signed certificate automatically.
/// 
/// REAL-WORLD CONSIDERATION: In some deployment scenarios (behind a load balancer or
/// reverse proxy), you might handle HTTPS at the infrastructure level instead.
/// </summary>
app.UseHttpsRedirection();

/// <summary>
/// THE APPLICATION STARTUP - This is where your application actually begins running.
/// 
/// WHAT'S HAPPENING HERE:
/// - app.Run() = "Start the web server and begin listening for HTTP requests"
/// - This method blocks until the application is shut down
/// 
/// WHY THIS IS THE LAST LINE:
/// - All configuration must be complete before the application starts
/// - Once Run() is called, the configuration is locked
/// - The application will continue running until manually stopped
/// 
/// STUDENT REALITY CHECK: This line is where your application transitions from
/// "getting ready to run" to "actually running." Everything before this is setup.
/// 
/// HOSTING MODEL: By default, ASP.NET Core uses Kestrel as its web server, which is
/// a cross-platform, high-performance server suitable for production use.
/// </summary>
app.Run();


