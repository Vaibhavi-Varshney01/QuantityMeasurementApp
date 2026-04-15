using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace RepositoryService.DBContext;

public class QMADbContext : DbContext
{
    public QMADbContext(DbContextOptions<QMADbContext> options) : base(options) { }
    public DbSet<QuantityMeasurementEntity> QuantityMeasurements => Set<QuantityMeasurementEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<QuantityMeasurementEntity>())
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;
        return base.SaveChanges();
    }
}
