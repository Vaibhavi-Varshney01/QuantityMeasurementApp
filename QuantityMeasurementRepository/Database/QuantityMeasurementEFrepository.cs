using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository.Database;

public class QuantityMeasurementEFRepository : IQuantityMeasurementRepository
{
    private readonly QuantityMeasurementDbContext _context;

    public QuantityMeasurementEFRepository(QuantityMeasurementDbContext context)
        => _context = context;

    public void Save(QuantityMeasurementEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        _context.QuantityMeasurements.Add(entity);
        _context.SaveChanges();
    }

    public List<QuantityMeasurementEntity> GetAllMeasurements()
        => _context.QuantityMeasurements
            .OrderByDescending(e => e.CreatedAt)
            .AsNoTracking()
            .ToList();

    public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operation)
        => _context.QuantityMeasurements
            .Where(e => e.OperationType == operation)
            .AsNoTracking()
            .ToList();

    public List<QuantityMeasurementEntity> GetMeasurementsByType(string type)
        => _context.QuantityMeasurements
            .Where(e => e.MeasurementType == type)
            .AsNoTracking()
            .ToList();

    public int GetTotalCount() => _context.QuantityMeasurements.Count();

    public void DeleteAll()
    {
        _context.QuantityMeasurements.RemoveRange(_context.QuantityMeasurements);
        _context.SaveChanges();
    }
}
