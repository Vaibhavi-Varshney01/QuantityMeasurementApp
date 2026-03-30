using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepository
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        List<QuantityMeasurementEntity> GetAllMeasurements();

        List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType);

        List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType);

        int GetTotalCount();

        void DeleteAll();

        // Default — DB repo overrides with real pool stats
        string GetPoolStatistics() => "No pool (cache repository)";

        // Default — DB repo overrides to close connections
        void ReleaseResources() { }
    }
}

