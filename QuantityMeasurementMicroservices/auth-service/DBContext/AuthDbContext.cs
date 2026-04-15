using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace AuthService.DBContext;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    public DbSet<UserEntity> Users => Set<UserEntity>();
}
