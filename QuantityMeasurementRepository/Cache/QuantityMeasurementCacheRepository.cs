using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository _instance;
        private readonly List<QuantityMeasurementEntity> _cache;

        private QuantityMeasurementCacheRepository()
        {
            _cache = new List<QuantityMeasurementEntity>();
        }

        public static QuantityMeasurementCacheRepository Instance =>
            _instance ??= new QuantityMeasurementCacheRepository();

        public void Save(QuantityMeasurementEntity entity)
        {
            _cache.Add(entity);
        }

        public IEnumerable<QuantityMeasurementEntity> GetAll()
        {
            return _cache;
        }
    }
}
