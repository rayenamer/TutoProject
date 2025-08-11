using System;
using Microsoft.EntityFrameworkCore;
using TutoProject.Data;
using TutoProject.Models;

namespace TutoProject.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.FindAsync(id);

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var prod = await GetByIdAsync(id);
        if (prod != null)
        {
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
        }
    }


    //search function
    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
                             .Where(p => p.Name != null && EF.Functions.Like(p.Name, $"%{searchTerm}%"))
                             .ToListAsync();
    }

}
