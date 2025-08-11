using System;
using TutoProject.Models;
using TutoProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text;
namespace TutoProject.Controllers;

/// <summary>
/// THE PRODUCTS API CONTROLLER - This is your application's interface to the outside world.
/// Think of this class as the "front desk" that handles ALL external requests for product data.
/// 
/// WHAT THIS CLASS DOES:
/// - Exposes HTTP endpoints for product operations (GET, POST, PUT, DELETE)
/// - Translates HTTP requests into repository method calls
/// - Formats repository data into proper HTTP responses
/// - Handles HTTP-specific concerns like status codes and content types
/// 
/// WHY THIS MATTERS:
/// Without this controller, your application would have no way to communicate with external clients.
/// This is the bridge that connects your business logic to the web, mobile apps, or other services.
/// 
/// CONTROLLER PATTERN BENEFITS:
/// - Separation of Concerns: API logic is separate from business logic
/// - Testability: Controllers can be tested independently of repositories
/// - Maintainability: All API endpoints are centralized in one place
/// - Security: Controllers handle authentication, validation, and authorization
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    /// <summary>
    /// THE REPOSITORY REFERENCE - This is your gateway to product data operations.
    /// 
    /// WHAT THIS FIELD REPRESENTS:
    /// - private readonly = "This field can't be changed after construction and is only accessible within this class"
    /// - IProductRepository = "The interface that defines all product data operations"
    /// - _repo = "The actual repository instance (underscore prefix indicates private field)"
    /// 
    /// WHY INTERFACE INSTEAD OF CONCRETE CLASS:
    /// - Dependency Inversion: Controller depends on abstraction, not implementation
    /// - Testability: You can mock the repository for unit tests
    /// - Flexibility: You can swap repository implementations without changing controller code
    /// 
    /// STUDENT ALERT: The underscore prefix (_repo) is a C# naming convention that indicates
    /// "this is a private field." It helps distinguish fields from local variables.
    /// </summary>
    private readonly IProductRepository _repo;

    /// <summary>
    /// THE CONSTRUCTOR - This is where dependency injection happens.
    /// 
    /// WHAT'S HAPPENING HERE:
    /// - IProductRepository repo = "Parameter that will be injected by the dependency injection container"
    /// - _repo = repo = "Assign the injected repository to our private field"
    /// 
    /// WHY DEPENDENCY INJECTION:
    /// - Your Program.cs configures the repository implementation
    /// - The DI container creates an IProductRepository instance
    /// - It automatically passes that instance to this constructor
    /// - You don't create the repository manually - the framework does it for you
    /// 
    /// STUDENT REALITY CHECK: This constructor gets called automatically when a request arrives.
    /// You never call it directly - the framework instantiates controllers as needed.
    /// </summary>
    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// GET ALL PRODUCTS ENDPOINT - Retrieves the complete product catalog.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Handles HTTP GET requests to /api/products
    /// - Calls the repository's GetAllAsync method
    /// - Returns all products as JSON with 200 OK status
    /// 
    /// WHY Expression-bodied member (=>):
    /// - => = "This is a simple method that just returns a value"
    /// - await _repo.GetAllAsync() = "Get all products from the repository"
    /// - This is a concise way to write simple methods
    /// 
    /// STUDENT ALERT: This endpoint returns ALL products at once. For large catalogs,
    /// consider implementing pagination to improve performance and reduce bandwidth usage.
    /// 
    /// REAL-WORLD USAGE: Product catalog pages, admin dashboards, mobile app product listings
    /// 
    /// HTTP DETAILS:
    /// - Method: GET
    /// - URL: /api/products
    /// - Auth: Typically public or requires basic authentication
    /// - Response: 200 OK with JSON array of products
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAll() => await _repo.GetAllAsync();

    /// <summary>
    /// GET PRODUCT BY ID ENDPOINT - Retrieves a specific product by its unique identifier.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Handles HTTP GET requests to /api/products/{id}
    /// - Calls the repository's GetByIdAsync method with the provided ID
    /// - Returns the product as JSON with 200 OK status if found
    /// - Returns 404 Not Found status if no product exists with that ID
    /// 
    /// WHY ActionResult<Product>:
    /// - ActionResult<T> = "Can return either T or an HTTP status result"
    /// - This allows returning either a Product or a NotFound result
    /// - More flexible than just returning Product (which can't handle missing products)
    /// 
    /// STUDENT ALERT: The ternary operator (? :) is a concise way to handle the found/not found logic.
    /// It's equivalent to an if-else statement but more compact for simple conditions.
    /// 
    /// REAL-WORLD USAGE: Product detail pages, edit forms, quick lookups
    /// 
    /// HTTP DETAILS:
    /// - Method: GET
    /// - URL: /api/products/{id} (e.g., /api/products/5)
    /// - Auth: Typically public or requires basic authentication
    /// - Response: 200 OK with JSON product or 404 Not Found
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var prod = await _repo.GetByIdAsync(id);
        return prod == null ? NotFound() : Ok(prod);
    }

    /// <summary>
    /// CREATE PRODUCT ENDPOINT - Adds a new product to the catalog.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Handles HTTP POST requests to /api/products
    /// - Extracts the product data from the request body
    /// - Calls the repository's AddAsync method to save the product
    /// - Returns 201 Created status with the new product's location and data
    /// 
    /// WHY CreatedAtAction:
    /// - CreatedAtAction = "Return 201 Created with a location header pointing to the new resource"
    /// - nameof(GetById) = "Use the GetById method to generate the location URL"
    /// - new { id = product.Id } = "Pass the new product's ID to the GetById route"
    /// - product = "Include the complete product in the response body"
    /// 
    /// STUDENT ALERT: The 201 Created status is the proper response for successful resource creation.
    /// It includes a Location header that tells the client where to find the new resource.
    /// 
    /// REAL-WORLD USAGE: Admin panels, product management systems, inventory systems
    /// 
    /// HTTP DETAILS:
    /// - Method: POST
    /// - URL: /api/products
    /// - Body: JSON product object (without ID)
    /// - Auth: Typically requires authentication and authorization
    /// - Response: 201 Created with Location header and JSON product (with ID)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await _repo.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// UPDATE PRODUCT ENDPOINT - Modifies an existing product in the catalog.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Handles HTTP PUT requests to /api/products/{id}
    /// - Validates that the URL ID matches the product ID in the body
    /// - Calls the repository's UpdateAsync method to update the product
    /// - Returns 204 No Content status on success
    /// 
    /// WHY Check ID Match:
    /// - if (id != product.Id) return BadRequest() = "Ensure URL ID and body ID match"
    /// - This prevents accidentally updating the wrong product
    /// - It's a common API design pattern for consistency and security
    /// 
    /// STUDENT ALERT: The 204 No Content status is appropriate when the operation succeeds
    /// but there's no need to return data. It's more efficient than returning the updated product.
    /// 
    /// REAL-WORLD USAGE: Admin panels, product management, price updates
    /// 
    /// HTTP DETAILS:
    /// - Method: PUT
    /// - URL: /api/products/{id} (e.g., /api/products/5)
    /// - Body: Complete JSON product object (with ID)
    /// - Auth: Typically requires authentication and authorization
    /// - Response: 204 No Content on success, 400 Bad Request if IDs don't match
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        await _repo.UpdateAsync(product);
        return NoContent();
    }

    /// <summary>
    /// DELETE PRODUCT ENDPOINT - Removes a product from the catalog.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Handles HTTP DELETE requests to /api/products/{id}
    /// - Calls the repository's DeleteAsync method with the provided ID
    /// - Returns 204 No Content status on success
    /// 
    /// WHY No Existence Check:
    /// - Unlike GetById, this method doesn't check if the product exists
    /// - The repository handles non-existent products gracefully
    /// - This simplifies the controller code and reduces database queries
    /// 
    /// STUDENT ALERT: In a production application, you might want to add error handling
    /// to catch and handle exceptions from the repository (e.g., if deletion fails).
    /// 
    /// REAL-WORLD USAGE: Admin panels, product management, catalog maintenance
    /// 
    /// HTTP DETAILS:
    /// - Method: DELETE
    /// - URL: /api/products/{id} (e.g., /api/products/5)
    /// - Auth: Typically requires authentication and authorization
    /// - Response: 204 No Content on success
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// SEARCH PRODUCTS ENDPOINT - Finds products matching a search term.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Handles HTTP GET requests to /api/products/search?query={term}
    /// - Calls the repository's SearchAsync method with the provided query
    /// - Returns matching products as JSON with 200 OK status
    /// 
    /// WHY Separate Endpoint:
    /// - /search?query= = "Clear, RESTful way to implement search functionality"
    /// - Keeps the main GET endpoint clean and focused on retrieving all products
    /// - Allows for future search enhancements (filters, sorting, etc.)
    /// 
    /// STUDENT ALERT: This endpoint returns ALL matching products. For potentially large
    /// result sets, consider adding pagination parameters (page, pageSize).
    /// 
    /// REAL-WORLD USAGE: Product search bars, filtered product listings, category views
    /// 
    /// HTTP DETAILS:
    /// - Method: GET
    /// - URL: /api/products/search?query={term} (e.g., /api/products/search?query=laptop)
    /// - Auth: Typically public or requires basic authentication
    /// - Response: 200 OK with JSON array of matching products
    /// </summary>
    [HttpGet("search")]
    public async Task<IEnumerable<Product>> Search(string query)
    {
        return await _repo.SearchAsync(query);
    }

    /// <summary>
    /// EXPORT PRODUCTS TO CSV ENDPOINT - Generates a downloadable CSV file of all products.
    /// 
    /// WHAT THIS METHOD DOES:
    /// - Handles HTTP GET requests to /api/products/export/csv
    /// - Retrieves all products from the repository
    /// - Builds a CSV string with product data (Id, Name, Price)
    /// - Returns the CSV as a downloadable file
    /// 
    /// WHY StringBuilder:
    /// - StringBuilder = "Efficient way to build strings incrementally"
    /// - More memory-efficient than string concatenation for large datasets
    /// - Especially important when building potentially large CSV files
    /// 
    /// STUDENT ALERT: The File() method is a special controller method that returns
    /// file content with the specified MIME type and suggested filename.
    /// 
    /// REAL-WORLD USAGE: Data exports, reports, backups, integrations with other systems
    /// 
    /// HTTP DETAILS:
    /// - Method: GET
    /// - URL: /api/products/export/csv
    /// - Auth: Typically requires authentication and authorization
    /// - Response: 200 OK with CSV file download (Content-Type: text/csv)
    /// 
    /// PERFORMANCE CONSIDERATION: For large catalogs, consider implementing this as a
    /// background job that generates the CSV asynchronously and notifies the user when ready.
    /// </summary>
    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportToCsv()
    {
        var products = await _repo.GetAllAsync(); // Assuming the method returns a list of products
        var csv = new StringBuilder();
        csv.AppendLine("Id,Name,Price");

        foreach (var product in products)
        {
            csv.AppendLine($"{product.Id},{product.Name},{product.Price}");
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "products.csv");
    }
}