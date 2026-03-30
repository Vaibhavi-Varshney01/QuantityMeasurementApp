using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementBusinessLayer.Helper;
using QuantityMeasurementBusinessLayer.Interface;
using QuantityMeasurementBusinessLayer.Service;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Database;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
builder.Services.AddScoped<JwtHelper>();

// Database
builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository (optionally cached via Redis)
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrWhiteSpace(redisConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "QuantityMeasurement:";
    });

    builder.Services.AddScoped<QuantityMeasurementEFRepository>();
    builder.Services.AddScoped<IQuantityMeasurementRepository>(sp =>
        new QuantityMeasurementRedisCacheRepository(
            sp.GetRequiredService<QuantityMeasurementEFRepository>(),
            sp.GetRequiredService<IDistributedCache>()));
}
else
{
    builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementEFRepository>();
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health checks
builder.Services.AddHealthChecks();

builder.Services.AddControllers();

var app = builder.Build();

// Global error handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            timestamp = DateTime.UtcNow,
            status    = 500,
            error     = "Internal Server Error",
            message   = app.Environment.IsDevelopment() ? exception?.Message : null,
            path      = context.Request.Path.Value
        });
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// Default landing endpoint (so http://localhost:<port>/ doesn't show 404)
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapHealthChecks("/actuator/health");
app.MapControllers();
app.Run();

public partial class Program { }
