using System;
using TutoProject.Models;

namespace TutoProject.Repositories;

/// <summary>
/// THE PRODUCT REPOSITORY INTERFACE - This is your data access contract. This interface defines
/// EXACTLY how your application will interact with product data, regardless of where that data lives.
/// 
/// WHAT THIS INTERFACE REPRESENTS:
/// - A contract for all product data operations (CRUD + Search)
/// - A blueprint for data access that hides database complexity
/// - A way to swap data sources without changing your business logic
/// - The foundation of the Repository Pattern
/// 
/// WHY THIS MATTERS:
/// This interface separates your business logic from your data access logic. Your controllers
/// and services can work with products without knowing if they're stored in SQLite, SQL Server,
/// or even a web API. This is called "abstraction" and it's a fundamental principle of good software design.
/// 
/// STUDENT REALITY CHECK: This is NOT just an interface - it's your data access strategy.
/// It defines the rules that any product data source must follow.
/// 
/// REPOSITORY PATTERN BENEFITS:
/// - Testability: You can easily mock this interface for unit tests
/// - Flexibility: Switch from SQLite to SQL Server without changing business code
/// - Maintainability: All data access logic is centralized in one place
/// - Separation of Concerns: Business logic doesn't care about database details
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// GET ALL PRODUCTS - Retrieves every single product from your data source.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Fetches all products from the database/memory/API
    /// - Returns them as a collection that can be iterated through
    /// - Handles the complexity of data retrieval behind the scenes
    /// 
    /// WHY ASYNC (Task<IEnumerable<Product>>):
    /// - Task = "This operation might take time (database queries can be slow)"
    /// - async/await = "Don't block the main thread while waiting for data"
    /// - IEnumerable<Product> = "A collection of products that you can loop through"
    /// 
    /// STUDENT ALERT: The 'Async' suffix is a naming convention that tells developers
    /// "This method returns a Task and should be awaited." Always await async methods!
    /// 
    /// REAL-WORLD USAGE: Displaying a product catalog, generating reports, admin dashboards
    /// 
    /// PERFORMANCE CONSIDERATION: This method loads ALL products into memory. For large datasets,
    /// you might want pagination (GetProductsAsync(page, pageSize)).
    /// </summary>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// GET PRODUCT BY ID - Retrieves a specific product using its unique identifier.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Searches for a product with the exact ID you provide
    /// - Returns the product if found, null if not found
    /// - This is the fastest way to retrieve a specific product
    /// 
    /// WHY Product? (nullable return):
    /// - Product? = "This method might return null if no product exists with that ID"
    /// - This prevents crashes when someone searches for a non-existent product
    /// - Always check for null before using the returned product
    /// 
    /// STUDENT ALERT: This method is your "fast lookup" method. It's much faster than
    /// searching by name because databases index ID columns automatically.
    /// 
    /// REAL-WORLD USAGE: Product detail pages, editing existing products, quick lookups
    /// 
    /// ERROR HANDLING: Always check if the result is null before proceeding:
    /// var product = await repository.GetByIdAsync(123);
    /// if (product != null) { /* use the product */ }
    /// </summary>
    Task<Product?> GetByIdAsync(int id);

    /// <summary>
    /// ADD NEW PRODUCT - Creates a new product in your data source.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Takes a Product object and stores it permanently
    /// - The product object should NOT have an ID set (database will generate it)
    /// - After calling this method, the product exists in your data source
    /// 
    /// WHY VOID RETURN (Task):
    /// - Task = "This operation completes asynchronously"
    /// - No return value = "We don't need anything back, just confirmation it worked"
    /// - If something goes wrong, this method will throw an exception
    /// 
    /// STUDENT ALERT: This method is for CREATING new products. If you want to update
    /// an existing product, use UpdateAsync instead.
    /// 
    /// REAL-WORLD USAGE: Admin panels adding new products, bulk imports, user submissions
    /// 
    /// VALIDATION: The Product object should be validated before calling this method.
    /// Check that Name is not empty, Price is positive, etc.
    /// </summary>
    Task AddAsync(Product product);

    /// <summary>
    /// UPDATE EXISTING PRODUCT - Modifies an existing product in your data source.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Takes a Product object and updates the existing record
    /// - The product object MUST have a valid ID set
    /// - All properties of the product will be updated
    /// 
    /// WHY UPDATE VS ADD:
    /// - AddAsync = "Create a brand new product"
    /// - UpdateAsync = "Modify an existing product"
    /// - The ID field determines which existing product to update
    /// 
    /// STUDENT ALERT: This method will fail if you try to update a product that doesn't exist.
    /// Always check if the product exists before calling UpdateAsync.
    /// 
    /// REAL-WORLD USAGE: Admin panels editing products, price updates, inventory changes
    /// 
    /// IMPORTANT: The Product object you pass must have ALL the data you want to keep.
    /// If you only set Name and Price, other fields might get overwritten with default values.
    /// </summary>
    Task UpdateAsync(Product product);

    /// <summary>
    /// DELETE PRODUCT - Removes a product from your data source permanently.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Finds the product with the specified ID and removes it
    /// - This action is PERMANENT and cannot be undone
    /// - The product no longer exists in your data source
    /// 
    /// WHY ID PARAMETER INSTEAD OF PRODUCT OBJECT:
    /// - We only need the ID to identify which product to delete
    /// - No need to load the entire product object just to delete it
    /// - More efficient than passing a full Product object
    /// 
    /// STUDENT ALERT: This is a DESTRUCTIVE operation. Once you call this method,
    /// the product is gone forever. Consider soft deletes for production applications.
    /// 
    /// REAL-WORLD USAGE: Admin panels removing discontinued products, cleanup operations
    /// 
    /// SAFETY CONSIDERATION: In real applications, you might want to:
    /// - Check if the product exists before deleting
    /// - Verify the user has permission to delete
    /// - Log the deletion for audit purposes
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// SEARCH PRODUCTS - Finds products that match your search criteria.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Searches through product names and potentially other fields
    /// - Returns a collection of products that match the search term
    /// - The search is typically case-insensitive and partial-match
    /// 
    /// WHY SEARCH VS GET ALL:
    /// - GetAllAsync = "Give me everything" (can be slow and memory-intensive)
    /// - SearchAsync = "Give me only what I'm looking for" (faster and more focused)
    /// - Search results are typically smaller and more relevant
    /// 
    /// STUDENT ALERT: This method enables user-friendly features like search bars and filters.
    /// It's what makes your app feel responsive and user-friendly.
    /// 
    /// REAL-WORLD USAGE: Product search bars, category filters, admin product management
    /// 
    /// SEARCH IMPLEMENTATION: The actual search logic depends on your data source:
    /// - SQLite: Uses LIKE queries with wildcards
    /// - SQL Server: Can use full-text search for better performance
    /// - In-Memory: String.Contains() for simple searches
    /// 
    /// PERFORMANCE NOTE: Search operations can be slow on large datasets. Consider:
    /// - Adding database indexes on searchable fields
    /// - Implementing pagination for large result sets
    /// - Using more sophisticated search engines for complex requirements
    /// </summary>
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
}
