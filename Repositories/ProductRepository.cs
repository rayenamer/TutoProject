using System;
using Microsoft.EntityFrameworkCore;
using TutoProject.Data;
using TutoProject.Models;

namespace TutoProject.Repositories;

/// <summary>
/// THE PRODUCT REPOSITORY IMPLEMENTATION - This is where the rubber meets the road. This class
/// implements the IProductRepository interface and provides the ACTUAL data access logic using
/// Entity Framework Core.
/// 
/// WHAT THIS CLASS DOES:
/// - Implements all the methods defined in IProductRepository
/// - Uses Entity Framework to interact with your SQLite database
/// - Handles the complex database operations behind the scenes
/// - Provides a clean, simple interface for your business logic
/// 
/// WHY THIS MATTERS:
/// This class is the bridge between your interface (what you promise to do) and your database
/// (how you actually do it). It's where you see the Repository Pattern in action - your
/// controllers don't know or care that you're using Entity Framework or SQLite.
/// 
/// STUDENT REALITY CHECK: This is NOT just a class - it's your data access implementation.
/// It's where you see how the abstract interface becomes concrete database operations.
/// 
/// IMPLEMENTATION PATTERN:
/// - Constructor injection: Gets database context from dependency injection
/// - Async operations: All methods are async for better performance
/// - Entity Framework: Uses EF Core for database operations
/// - Error handling: Gracefully handles missing data and edge cases
/// </summary>
public class ProductRepository : IProductRepository
{
    /// <summary>
    /// THE DATABASE CONTEXT - This is your direct connection to the database.
    /// 
    /// WHAT THIS FIELD REPRESENTS:
    /// - private readonly = "This field can't be changed after construction and is only accessible within this class"
    /// - AppDbContext = "The Entity Framework context that manages your database connection"
    /// - _context = "The actual instance of your database context (underscore prefix indicates private field)"
    /// 
    /// WHY PRIVATE READONLY:
    /// - private = "Only this class can access this field"
    /// - readonly = "This field can only be set in the constructor, never changed after"
    /// - This prevents accidental modification and ensures thread safety
    /// 
    /// STUDENT ALERT: The underscore prefix (_context) is a C# naming convention that indicates
    /// "this is a private field." It helps distinguish fields from local variables.
    /// 
    /// REAL-WORLD USAGE: This field is used in every method to perform database operations.
    /// It's your gateway to all the data in your Products table.
    /// </summary>
    private readonly AppDbContext _context;

    /// <summary>
    /// THE CONSTRUCTOR - This is where dependency injection happens.
    /// 
    /// WHAT'S HAPPENING HERE:
    /// - AppDbContext context = "Parameter that will be injected by the dependency injection container"
    /// - _context = context = "Assign the injected context to our private field"
    /// 
    /// WHY DEPENDENCY INJECTION:
    /// - Your Program.cs configures the database connection
    /// - The DI container creates an AppDbContext instance
    /// - It automatically passes that instance to this constructor
    /// - You don't create the context manually - the framework does it for you
    /// 
    /// STUDENT REALITY CHECK: This constructor gets called automatically when you register
    /// this repository in your dependency injection container. You never call it directly.
    /// 
    /// BENEFITS OF THIS PATTERN:
    /// - Testability: You can inject a mock context for unit tests
    /// - Configuration: Database settings are centralized in Program.cs
    /// - Lifecycle management: The DI container manages when contexts are created/destroyed
    /// </summary>
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET ALL PRODUCTS IMPLEMENTATION - Retrieves every product from the database.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Accesses the Products DbSet from your database context
    /// - Converts the database query to a List<Product> in memory
    /// - Returns the complete collection asynchronously
    /// 
    /// WHY Expression-bodied member (=>):
    /// - => = "This is a simple method that just returns a value"
    /// - await _context.Products.ToListAsync() = "Get all products and convert to list"
    /// - This is a concise way to write simple methods
    /// 
    /// STUDENT ALERT: ToListAsync() loads ALL products into memory at once. For large datasets,
    /// this can cause performance issues. Consider pagination for production applications.
    /// 
    /// REAL-WORLD USAGE: Product catalogs, admin dashboards, data exports
    /// 
    /// PERFORMANCE NOTE: This method executes a SELECT * FROM Products query. If you have
    /// thousands of products, consider implementing pagination or filtering.
    /// </summary>
    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.ToListAsync();

    /// <summary>
    /// GET PRODUCT BY ID IMPLEMENTATION - Fast lookup of a specific product.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Uses Entity Framework's FindAsync method for optimal performance
    /// - Searches by primary key (ID) which is always indexed
    /// - Returns null if no product exists with that ID
    /// 
    /// WHY FindAsync INSTEAD OF WHERE:
    /// - FindAsync = "Optimized for primary key lookups, uses database indexes"
    /// - Where = "General-purpose filtering, can be slower for ID searches"
    /// - FindAsync is the fastest way to retrieve a single record by ID
    /// 
    /// STUDENT ALERT: This method is your "fast path" for retrieving products. It's much
    /// faster than searching by name because databases automatically index ID columns.
    /// 
    /// REAL-WORLD USAGE: Product detail pages, quick lookups, data validation
    /// 
    /// ERROR HANDLING: This method returns null for non-existent IDs, so always check
    /// the result before using it in your business logic.
    /// </summary>
    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.FindAsync(id);

    /// <summary>
    /// ADD NEW PRODUCT IMPLEMENTATION - Creates a new product in the database.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Adds the product to Entity Framework's change tracking
    /// - Calls SaveChangesAsync to persist the changes to the database
    /// - The database automatically generates a new ID for the product
    /// 
    /// WHY TWO-STEP PROCESS (Add + SaveChanges):
    /// - Add() = "Tell Entity Framework to track this new product"
    /// - SaveChangesAsync() = "Actually write the changes to the database"
    /// - This two-step process allows for transaction management and validation
    /// 
    /// STUDENT ALERT: SaveChangesAsync() is when your data actually gets written to the database.
    /// If you call Add() but forget SaveChangesAsync(), your product won't be saved!
    /// 
    /// REAL-WORLD USAGE: Admin panels, bulk imports, user submissions
    /// 
    /// VALIDATION: Entity Framework will validate the data before saving. If there are
    /// validation errors (like null names or negative prices), SaveChangesAsync() will throw an exception.
    /// </summary>
    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// UPDATE EXISTING PRODUCT IMPLEMENTATION - Modifies an existing product.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Tells Entity Framework that this product has been modified
    /// - Calls SaveChangesAsync to persist the changes to the database
    /// - Updates all properties of the existing product record
    /// 
    /// WHY Update() METHOD:
    /// - Update() = "Tell EF this product has changed and needs to be saved"
    /// - Entity Framework tracks which properties have been modified
    /// - Only changed properties are included in the UPDATE SQL query
    /// 
    /// STUDENT ALERT: The product object you pass MUST have a valid ID set. If the ID
    /// doesn't exist in the database, this method will fail or create a duplicate.
    /// 
    /// REAL-WORLD USAGE: Admin panels editing products, price updates, inventory changes
    /// 
    /// IMPORTANT: Entity Framework will update ALL properties of the product. If you only
    /// set Name and Price, other fields might get overwritten with default values.
    /// </summary>
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// DELETE PRODUCT IMPLEMENTATION - Removes a product from the database.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - First checks if the product exists by calling GetByIdAsync
    /// - If the product exists, removes it from Entity Framework's change tracking
    /// - Calls SaveChangesAsync to persist the deletion to the database
    /// 
    /// WHY CHECK EXISTENCE FIRST:
    /// - Prevents errors when trying to delete non-existent products
    /// - Provides graceful handling of invalid delete requests
    /// - Follows the principle of "fail fast, fail safely"
    /// 
    /// WHY GetByIdAsync INSTEAD OF DIRECT REMOVE:
    /// - GetByIdAsync validates that the product exists
    /// - It also gives us the full product object for logging or confirmation
    /// - More robust than trying to remove a potentially non-existent entity
    /// 
    /// STUDENT ALERT: This is a SAFETY-FIRST implementation. In production, you might also want to:
    /// - Check user permissions before deletion
    /// - Log the deletion for audit purposes
    /// - Implement soft deletes instead of hard deletes
    /// 
    /// REAL-WORLD USAGE: Admin panels removing discontinued products, cleanup operations
    /// 
    /// PERFORMANCE NOTE: This method makes two database calls (one to check existence,
    /// one to delete). For high-performance applications, you might optimize this.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var prod = await GetByIdAsync(id);
        if (prod != null)
        {
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// SEARCH PRODUCTS IMPLEMENTATION - Finds products matching search criteria.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Uses LINQ Where clause to filter products by name
    /// - Checks for null names to prevent crashes
    /// - Uses EF.Functions.Like for database-optimized pattern matching
    /// - Converts results to a List<Product> in memory
    /// 
    /// WHY NULL CHECK (p.Name != null):
    /// - Prevents crashes when searching through products with null names
    /// - The ? in string? Name means names can be null
    /// - This is defensive programming - always check for null before using nullable properties
    /// 
    /// WHY EF.Functions.Like INSTEAD OF String.Contains:
    /// - EF.Functions.Like = "Database-level pattern matching, very fast"
    /// - String.Contains = "In-memory filtering, slower for large datasets"
    /// - EF.Functions.Like generates optimized SQL LIKE queries
    /// 
    /// STUDENT ALERT: The % wildcards in $"%{searchTerm}%" mean:
    /// - %searchTerm% = "Find searchTerm anywhere in the name"
    /// - searchTerm% = "Find names that start with searchTerm"
    /// - %searchTerm = "Find names that end with searchTerm"
    /// 
    /// REAL-WORLD USAGE: Product search bars, category filters, admin product management
    /// 
    /// PERFORMANCE CONSIDERATIONS:
    /// - This method loads all matching products into memory
    /// - For large result sets, consider implementing pagination
    /// - Consider adding database indexes on the Name column for faster searches
    /// - The search is case-sensitive by default (depends on database collation)
    /// </summary>
    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
                             .Where(p => p.Name != null && EF.Functions.Like(p.Name, $"%{searchTerm}%"))
                             .ToListAsync();
    }
}
