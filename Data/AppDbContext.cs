using System;
using Microsoft.EntityFrameworkCore; 
using TutoProject.Models;

namespace TutoProject.Data;

/// <summary>
/// THE CORE DATABASE CONTEXT - This is your application's direct connection to the database.
/// Think of this class as the "command center" that manages ALL database operations.
/// 
/// WHAT THIS CLASS DOES:
/// - Creates the bridge between your C# code and the actual database
/// - Manages database connections, transactions, and data synchronization
/// - Translates your C# objects into database records and vice versa
/// - Handles all the complex SQL operations behind the scenes
/// 
/// WHY THIS MATTERS:
/// Without this class, your application would be completely disconnected from any data storage.
/// This is the foundation that makes your app actually useful and persistent.
/// </summary>
public class AppDbContext: DbContext
{
    /// <summary>
    /// THE PRODUCTS TABLE - This property represents your Products table in the database.
    /// 
    /// WHAT THIS MEANS:
    /// - DbSet<Product> = "A collection of Product objects that maps to a database table"
    /// - Products = "The actual name of your table in the database"
    /// - This property gives you CRUD operations (Create, Read, Update, Delete) on your products
    /// 
    /// HOW TO USE IT:
    /// - To get all products: context.Products.ToList()
    /// - To add a product: context.Products.Add(newProduct)
    /// - To find a product: context.Products.Find(id)
    /// - To delete: context.Products.Remove(product)
    /// 
    /// STUDENT ALERT: This is NOT a regular List<Product>. It's a special Entity Framework
    /// collection that automatically syncs with your database when you call SaveChanges().
    /// </summary>
    public DbSet<Product> Products { get; set; } 
 
    /// <summary>
    /// THE CONSTRUCTOR - This is where the magic begins.
    /// 
    /// WHAT'S HAPPENING HERE:
    /// - DbContextOptions<AppDbContext> options = "Configuration settings for your database"
    /// - : base(options) = "Pass these settings to the parent DbContext class"
    /// 
    /// WHY THIS PATTERN:
    /// - Dependency Injection: Your app can configure different database connections
    /// - Testing: You can swap in a test database for unit tests
    /// - Flexibility: Same code can work with SQL Server, SQLite, PostgreSQL, etc.
    /// 
    /// STUDENT REALITY CHECK: This constructor gets called automatically by Entity Framework
    /// when you register this class in your Program.cs. You don't call it directly.
    /// 
    /// COMMON STUDENT CONFUSION: "Why do we need options?" Because Entity Framework needs to know:
    /// - What type of database you're using (SQLite, SQL Server, etc.)
    /// - Connection string (where is your database file/server?)
    /// - Any special settings (timeouts, logging, etc.)
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
