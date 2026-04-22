using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementBusinessLayer.Interface;
using QuantityMeasurementBusinessLayer.Service;
using QuantityMeasurementBusinessLayer.Helper;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Database;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Services & Dependency Injection
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementEFRepository>();
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
builder.Services.AddScoped<JwtHelper>();

// 2. Database Configuration
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? Environment.GetEnvironmentVariable("DATABASE_PRIVATE_URL")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrWhiteSpace(connectionString) &&
    (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
{
    try
    {
        var databaseUri = new Uri(connectionString);
        var userInfo = databaseUri.UserInfo.Split(':');
        var host = databaseUri.Host;
        var port = databaseUri.Port > 0 ? databaseUri.Port : 5432;
        var db = databaseUri.AbsolutePath.TrimStart('/');
        var user = userInfo[0];
        var pass = userInfo.Length > 1 ? userInfo[1] : "";
        connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass};SSL Mode=Require;Trust Server Certificate=true";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] Failed to parse connection string URI: {ex.Message}");
    }
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("[Warning] No valid connection string found. Falling back to localhost.");
    connectionString = "Host=localhost;Database=QuantityMeasurementDB;Username=postgres;Password=password";
}

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("QuantityMeasurementRepository")));

// 3. JWT Authentication (Dual Scheme: Custom JWT + Clerk)
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? Environment.GetEnvironmentVariable("Jwt__Key")
    ?? "ThisIsASecretKeyForQuantityMeasurementApp2026!";

var jwtIssuer   = builder.Configuration["Jwt:Issuer"]   ?? "QuantityMeasurementApp";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "QuantityMeasurementApp";
var clerkAuthority = builder.Configuration["Clerk:Authority"] ?? "https://real-weasel-59.clerk.accounts.dev";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtIssuer,
        ValidAudience            = jwtAudience,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
})
.AddJwtBearer("Clerk", options =>
{
    options.Authority = clerkAuthority;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer   = true,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization(options =>
{
    var combinedPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(
            JwtBearerDefaults.AuthenticationScheme, "Clerk")
        .RequireAuthenticatedUser()
        .Build();
    options.DefaultPolicy = combinedPolicy;
});

// 4. CORS — single policy, all frontend origins
// ✅ Only ONE AddCors call — fixes the duplicate conflict
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:3000"
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// 5. Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 6. Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    try
    {
        Console.WriteLine("[Database] Starting schema initialization...");
        var pending = db.Database.GetPendingMigrations().ToList();
        if (pending.Any())
        {
            Console.WriteLine($"[Database] Applying {pending.Count} pending migrations...");
            db.Database.Migrate();
        }
        db.Database.EnsureCreated();
        Console.WriteLine("[Database] Schema is ready.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Database] ERROR: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"[Database] Inner: {ex.InnerException.Message}");
    }
}

// 7. Global error handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        context.Response.ContentType  = "application/json";
        context.Response.StatusCode   = 500;
        await context.Response.WriteAsJsonAsync(new { message = exception?.Message });
    });
});

// 8. Swagger
app.UseSwagger();
app.UseSwaggerUI();

// ✅ CORRECT middleware order:
// CORS must come BEFORE Authentication and Authorization
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }