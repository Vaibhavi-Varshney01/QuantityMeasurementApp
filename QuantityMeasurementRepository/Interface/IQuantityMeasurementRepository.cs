using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepository
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        IEnumerable<QuantityMeasurementEntity> GetAll();
    }
}
