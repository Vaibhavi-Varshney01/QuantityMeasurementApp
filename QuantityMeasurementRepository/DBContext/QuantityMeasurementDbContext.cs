using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository;

public class QuantityMeasurementDbContext : DbContext
{
    public QuantityMeasurementDbContext(
        DbContextOptions<QuantityMeasurementDbContext> options) : base(options) { }

    public DbSet<QuantityMeasurementEntity> QuantityMeasurements
        => Set<QuantityMeasurementEntity>();

    // Add this line
    public DbSet<UserEntity> Users => Set<UserEntity>();

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<QuantityMeasurementEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;
        }
        return base.SaveChanges();
    }
}
 