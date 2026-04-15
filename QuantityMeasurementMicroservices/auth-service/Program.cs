using AuthService.DBContext;
using AuthService.Helper;
using AuthService.Interface;
using AuthService.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
builder.Services.AddScoped<JwtHelper>();

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
