using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartInventory_System.Data;
using SmartInventory_System.Services;
using SmartInventory_System.Services.Implementations;
using SmartInventory_System.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Register services (DI)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configure EF Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

// 3. Register custom services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<ILowStockAlertService, LowStockAlertService>();

// 4. Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()   // ? link Identity to EF
.AddDefaultTokenProviders();                        // ? needed for password reset, etc.

var app = builder.Build();

// 5. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ? authentication first, then authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
