using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository? _instance;
        private readonly List<QuantityMeasurementEntity> _cache;

        private QuantityMeasurementCacheRepository()
        {
            _cache = new List<QuantityMeasurementEntity>();
            Console.WriteLine("[CacheRepository] Initialised.");
        }

        public static QuantityMeasurementCacheRepository Instance =>
            _instance ??= new QuantityMeasurementCacheRepository();

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _cache.Add(entity);
            Console.WriteLine($"[CacheRepository] Saved: {entity.OperationType} | {entity.MeasurementType}");
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements() =>
            new List<QuantityMeasurementEntity>(_cache);

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType) =>
            _cache.Where(e => e.OperationType.Equals(
                operationType, StringComparison.OrdinalIgnoreCase)).ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType) =>
            _cache.Where(e => e.MeasurementType.Equals(
                measurementType, StringComparison.OrdinalIgnoreCase)).ToList();

        public int GetTotalCount() => _cache.Count;

        public void DeleteAll()
        {
            _cache.Clear();
            Console.WriteLine("[CacheRepository] All records deleted.");
        }
    }
}