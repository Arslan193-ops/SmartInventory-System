//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using SmartInventory_System.Data;
//using SmartInventory_System.Services;
//using SmartInventory_System.Services.Implementations;
//using SmartInventory_System.Services.Interfaces;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// 1. Register services (DI)
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new() { Title = "SmartInventory API", Version = "v1" });

//    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//        Description = "Enter 'Bearer' followed by your JWT token. Example: Bearer eyJhbGciOi..."
//    });

//    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//    {
//        {
//            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            {
//                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                {
//                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] {}
//        }
//    });
//});

//// 2. Configure EF Core DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

//// 3. Register custom services
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<IStockMovementService, StockMovementService>();
//builder.Services.AddScoped<ILowStockAlertService, LowStockAlertService>();



//// JWT Authentication

////builder.Services.AddAuthentication(options =>
////{
////    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
////})
////.AddJwtBearer(options =>
////{
////    var jwtKeyValue = builder.Configuration["Jwt:Key"];
////    if (string.IsNullOrEmpty(jwtKeyValue))
////    {
////        throw new InvalidOperationException("JWT Key is missing in configuration!");
////    }
////    options.TokenValidationParameters = new TokenValidationParameters
////    {
////        ValidateIssuer = true,
////        ValidateAudience = true,
////        ValidateLifetime = true,
////        ValidateIssuerSigningKey = true,
////        ValidIssuer = builder.Configuration["Jwt:Issuer"],
////        ValidAudience = builder.Configuration["Jwt:Audience"],
////        IssuerSigningKey = new SymmetricSecurityKey(
////            Encoding.UTF8.GetBytes(jwtKeyValue)
////        )
////    };
////});
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
//        };
//    });

//builder.Services.AddAuthorization();

//// 4. Configure Identity
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequiredLength = 6;
//})
//.AddEntityFrameworkStores<ApplicationDbContext>()   // ? link Identity to EF
//.AddDefaultTokenProviders();                        // ? needed for password reset, etc.

//var app = builder.Build();



//static async Task SeedRolesAndAdminAsync(IApplicationBuilder app)
//{
//    using var scope = app.ApplicationServices.CreateScope();

//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

//    string[] roles = new[] { "ADMIN", "USER" };

//    // Ensure roles exist
//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }

//    // Seed a default Admin user
//    string adminEmail = "admin@system.com";
//    string adminPassword = "Admin@123"; // ✅ You can change this

//    var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
//    if (existingAdmin == null)
//    {
//        var adminUser = new IdentityUser
//        {
//            UserName = adminEmail,
//            Email = adminEmail,
//            EmailConfirmed = true
//        };

//        var result = await userManager.CreateAsync(adminUser, adminPassword);
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(adminUser, "ADMIN");
//        }
//    }
//}


//// 5. Middleware
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseRouting();
//// ? authentication first, then authorization
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();
//await SeedRolesAndAdminAsync(app);

//app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartInventory_System.Data;
using SmartInventory_System.Services;
using SmartInventory_System.Services.Implementations;
using SmartInventory_System.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add controllers
builder.Services.AddControllers();

// 2. Swagger + JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "SmartInventory API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token. Example: Bearer eyJhbGciOi..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 3. Configure EF Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

// 4. Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 5. Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"];
    if (string.IsNullOrEmpty(jwtKey))
        throw new InvalidOperationException("JWT Key is missing in configuration!");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };


    options.Events = new JwtBearerEvents
{
    OnAuthenticationFailed = ctx =>
    {
        Console.WriteLine("JWT failed: " + ctx.Exception.Message);
        return Task.CompletedTask;
    },
    OnChallenge = ctx =>
    {
        Console.WriteLine("JWT challenge: " + ctx.ErrorDescription);
        return Task.CompletedTask;
    }
};
});

// 6. Add Authorization
builder.Services.AddAuthorization();

// 7. Register custom services (DI)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<ILowStockAlertService, LowStockAlertService>();

var app = builder.Build();

// 8. Middleware (ORDER IS CRITICAL)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   // 🔑 validate tokens
app.UseAuthorization();    // 🔑 enforce [Authorize]


app.MapControllers();

async Task SeedRolesAndAdminAsync(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // 1. Create roles
    string[] roles = new[] { "ADMIN", "USER" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // 2. Create default admin
    var adminEmail = "admin@system.com";
    var adminPassword = "Admin@123"; // change in production
    var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "ADMIN");
    }
}

// Call the seeding function
await SeedRolesAndAdminAsync(app);
// Run app
app.Run();

