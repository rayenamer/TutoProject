using System;

namespace TutoProject.Models;

/// <summary>
/// THE PRODUCT MODEL - This is your data blueprint. This class defines EXACTLY what a Product looks like
/// in your application and how it gets stored in the database.
/// 
/// WHAT THIS CLASS REPRESENTS:
/// - A real-world product (like a laptop, book, or service)
/// - The structure of your Products table in the database
/// - The template for creating, reading, updating, and deleting product data
/// 
/// WHY THIS MATTERS:
/// This class is the foundation of your entire product management system. Every product in your app
/// will be an instance of this class. Without this model, you have no way to represent product data.
/// 
/// STUDENT REALITY CHECK: This is NOT just a class - it's your data contract with the database.
/// Entity Framework uses this class to create your database table structure automatically.
/// </summary>
public class Product
{
    /// <summary>
    /// THE PRIMARY KEY - This is the unique identifier for every single product.
    /// 
    /// WHAT THIS MEANS:
    /// - int Id = "A whole number that uniquely identifies this product"
    /// - This property becomes the PRIMARY KEY in your database table
    /// - Every product MUST have a different Id value
    /// 
    /// WHY THIS EXISTS:
    /// - Database efficiency: Fast lookups when you search for specific products
    /// - Data integrity: No two products can have the same Id
    /// - Relationships: Other tables can reference this product using this Id
    /// 
    /// STUDENT ALERT: Entity Framework automatically generates this Id when you create a new product.
    /// You don't set this manually - the database handles it for you.
    /// 
    /// REAL-WORLD EXAMPLE: Think of this like a product SKU or barcode - every item has a unique number.
    /// </summary>
    public int Id { get; set; } 

    /// <summary>
    /// THE PRODUCT NAME - This is what your customers see and search for.
    /// 
    /// WHAT THIS MEANS:
    /// - string? Name = "Text that describes the product (the ? means it can be null)"
    /// - This becomes a column in your database table
    /// - Customers use this to identify what they're buying
    /// 
    /// WHY THE QUESTION MARK (?):
    /// - string? = "This property can be null (empty/not set)"
    /// - string = "This property MUST have a value"
    /// - In real apps, you might want to allow products without names initially
    /// 
    /// STUDENT REALITY CHECK: The ? is called "nullable reference type" - it's C#'s way of saying
    /// "This string might not have a value." Without the ?, you'd get compiler warnings.
    /// 
    /// REAL-WORLD EXAMPLE: "iPhone 15 Pro Max" or "The Great Gatsby" or "Premium Coffee Blend"
    /// </summary>
    public string ?Name { get; set; } 

    /// <summary>
    /// THE PRODUCT PRICE - This is the monetary value of your product.
    /// 
    /// WHAT THIS MEANS:
    /// - decimal Price = "A precise decimal number representing the cost"
    /// - This becomes a column in your database table
    /// - Used for calculations, display, and business logic
    /// 
    /// WHY DECIMAL INSTEAD OF DOUBLE:
    /// - decimal = "Precise decimal arithmetic (perfect for money)"
    /// - double = "Approximate floating-point arithmetic (can have rounding errors)"
    /// - NEVER use double for money - you'll lose pennies due to precision issues
    /// 
    /// STUDENT ALERT: This is a CRITICAL choice. Using double for prices is a common beginner mistake
    /// that can cost real money in production applications.
    /// 
    /// REAL-WORLD EXAMPLE: 29.99, 1499.99, 0.99 (prices are always decimal for precision)
    /// 
    /// BUSINESS LOGIC: This price field enables:
    /// - Total calculations in shopping carts
    /// - Discount applications
    /// - Tax calculations
    /// - Profit margin analysis
    /// </summary>
    public decimal Price { get; set; } 
}

