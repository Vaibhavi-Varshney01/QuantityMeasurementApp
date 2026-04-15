using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RepositoryService.Cache;
using RepositoryService.Database;
using RepositoryService.DBContext;
using RepositoryService.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QMADbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var redis = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrWhiteSpace(redis))
{
    builder.Services.AddStackExchangeRedisCache(o => { o.Configuration = redis; o.InstanceName = "QMA:"; });
    builder.Services.AddScoped<QMAEFRepository>();
    builder.Services.AddScoped<IQMARepository>(sp =>
        new QMARedisCacheRepository(
            sp.GetRequiredService<QMAEFRepository>(),
            sp.GetRequiredService<IDistributedCache>()));
}
else
    builder.Services.AddScoped<IQMARepository, QMAEFRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapHealthChecks("/actuator/health");
app.MapControllers();
app.Run();
