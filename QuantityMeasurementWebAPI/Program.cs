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

// 2. Database Configuration (Switch to PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

// Handle Render's postgres:// format if detected
if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres://"))
{
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');
    connectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. JWT Authentication Configuration (Dual Scheme: Custom + Clerk)
var jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("Jwt__Key");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "QuantityMeasurementApp";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "QuantityMeasurementApp";

var clerkAuthority = builder.Configuration["Clerk:Authority"] ?? "https://real-weasel-59.clerk.accounts.dev";

if (string.IsNullOrEmpty(jwtKey))
{
    jwtKey = "ThisIsASecretKeyForQuantityMeasurementApp2026!";
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => // Scheme 1: Custom JWT (Default)
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
})
.AddJwtBearer("Clerk", options => // Scheme 2: Clerk
{
    options.Authority = clerkAuthority;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false, // Clerk tokens usually don't have audience unless configured
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization(options =>
{
    // A combined policy that accepts either Custom JWT or Clerk tokens
    var combinedPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme,
        "Clerk")
        .RequireAuthenticatedUser()
        .Build();
    
    options.DefaultPolicy = combinedPolicy;
});

// 4. CORS Configuration
var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Global Error Handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new
        {
            message = exception?.Message
        });
    });
});

// Swagger (Always enabled for development/debugging on Render)
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ProductionPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }