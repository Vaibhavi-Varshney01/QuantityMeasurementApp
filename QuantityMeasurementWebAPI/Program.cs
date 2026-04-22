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
else
{
    Console.WriteLine("[Database] Using connection string from configuration (not a URI).");
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

// 4. CORS — allow any origin for troubleshooting connectivity
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
        // Note: AllowAnyOrigin() cannot be used with AllowCredentials()
    });
});

// 5. Health Checks
builder.Services.AddHealthChecks();

// 6. Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 7. Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<QuantityMeasurementDbContext>();
    try
    {
        Console.WriteLine("[Database] Starting schema initialization...");
        
        // 1. Try to run migrations
        var pending = db.Database.GetPendingMigrations().ToList();
        if (pending.Any())
        {
            Console.WriteLine($"[Database] Applying {pending.Count} pending migrations...");
            db.Database.Migrate();
        }
        
        // 2. Force check tables (last resort for Render/Postgres issues)
        try {
            db.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS ""quantity_measurements"" (
                    ""Id"" bigserial PRIMARY KEY,
                    ""OperationType"" varchar(50) NOT NULL,
                    ""MeasurementType"" varchar(50) NOT NULL DEFAULT 'Unknown',
                    ""Operand1"" varchar(200),
                    ""Operand2"" varchar(200),
                    ""Result"" varchar(200),
                    ""HasError"" boolean NOT NULL DEFAULT false,
                    ""ErrorMessage"" varchar(500),
                    ""CreatedAt"" timestamptz NOT NULL DEFAULT now()
                );
                CREATE TABLE IF NOT EXISTS ""users"" (
                    ""Id"" bigserial PRIMARY KEY,
                    ""Username"" varchar(100) NOT NULL,
                    ""PasswordHash"" varchar(255) NOT NULL,
                    ""Salt"" varchar(255) NOT NULL,
                    ""Role"" varchar(50) NOT NULL,
                    ""CreatedAt"" timestamptz NOT NULL DEFAULT now()
                );
            ");
            
            // Verify table existence by doing a simple count
            try {
                var count = db.Database.ExecuteSqlRaw("SELECT count(*) FROM \"quantity_measurements\"");
                Console.WriteLine($"[Database] Verified: 'quantity_measurements' table exists and is accessible.");
            } catch (Exception exCount) {
                Console.WriteLine($"[Database] CRITICAL: Table creation seemed to succeed but verification failed: {exCount.Message}");
            }

            Console.WriteLine("[Database] Schema check complete (Tables verified/created).");
        } catch (Exception ex) {
            Console.WriteLine($"[Database] Warning: Table verification failed: {ex.Message}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Database] CRITICAL ERROR during initialization: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"[Database] Inner Exception: {ex.InnerException.Message}");
    }
}

// 8. Global error handler
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

// 9. Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

// ✅ CORRECT middleware order:
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// 10. Endpoints
app.MapHealthChecks("/health").AllowAnonymous();
app.MapGet("/ping", () => Results.Ok(new { status = "Healthy", time = DateTime.UtcNow })).AllowAnonymous();

app.MapGet("/debug/tables", async (QuantityMeasurementDbContext db) => {
    try {
        var tables = new List<string>();
        using (var command = db.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";
            await db.Database.OpenConnectionAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    tables.Add(reader.GetString(0));
                }
            }
        }
        return Results.Ok(new { tables, connection = db.Database.GetDbConnection().Database });
    } catch (Exception ex) {
        return Results.Problem(ex.Message);
    }
}).AllowAnonymous();

app.MapControllers();

app.Run();

public partial class Program { }