using System;
using TutoProject.Models;
using TutoProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text;
namespace TutoProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repo;
    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> GetAll() => await _repo.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var prod = await _repo.GetByIdAsync(id);
        return prod == null ? NotFound() : Ok(prod);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await _repo.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        await _repo.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IEnumerable<Product>> Search(string query)
    {
        return await _repo.SearchAsync(query);
    }

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