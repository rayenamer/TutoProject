using System;
using Microsoft.EntityFrameworkCore; 
using TutoProject.Models;
namespace TutoProject.Data;

public class AppDbContext: DbContext
{
    public DbSet<Product> Products { get; set; } 
 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
