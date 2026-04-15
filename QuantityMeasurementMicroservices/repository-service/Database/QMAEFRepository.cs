using Microsoft.EntityFrameworkCore;
using RepositoryService.DBContext;
using RepositoryService.Interface;
using SharedModels.Entities;

namespace RepositoryService.Database;

public class QMAEFRepository : IQMARepository
{
    private readonly QMADbContext _ctx;
    public QMAEFRepository(QMADbContext ctx) => _ctx = ctx;

    public void Save(QuantityMeasurementEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _ctx.QuantityMeasurements.Add(entity);
        _ctx.SaveChanges();
    }
    public List<QuantityMeasurementEntity> GetAll()
        => _ctx.QuantityMeasurements.OrderByDescending(e => e.CreatedAt).AsNoTracking().ToList();
    public List<QuantityMeasurementEntity> GetByOperation(string op)
        => _ctx.QuantityMeasurements.Where(e => e.OperationType == op).AsNoTracking().ToList();
    public List<QuantityMeasurementEntity> GetByType(string type)
        => _ctx.QuantityMeasurements.Where(e => e.MeasurementType == type).AsNoTracking().ToList();
    public int GetTotalCount() => _ctx.QuantityMeasurements.Count();
    public void DeleteAll()
    {
        _ctx.QuantityMeasurements.RemoveRange(_ctx.QuantityMeasurements);
        _ctx.SaveChanges();
    }
}
