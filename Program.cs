using TutoProject.Data;
using Microsoft.EntityFrameworkCore;
using TutoProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Added database context with sqlite config (browse extensions and download it)
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=products.db"));
// register repositories so you can use them
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers(); // Map controllers to routes

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();





app.Run();


