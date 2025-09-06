using Microsoft.EntityFrameworkCore;
using SmartInventory_System.Data;
using SmartInventory_System.Services.Implementations;
using SmartInventory_System.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Register services (DI)
builder.Services.AddControllers(); // API controllers only
builder.Services.AddEndpointsApiExplorer(); // For minimal APIs & Swagger
builder.Services.AddSwaggerGen(); // Swagger/OpenAPI support

// 2. Configure EF Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// 3. Configure middleware (HTTP pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Map all API controllers

app.Run();
