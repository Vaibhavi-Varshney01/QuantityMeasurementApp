using SharedModels.Entities;

namespace RepositoryService.Interface;

public interface IQMARepository
{
    void Save(QuantityMeasurementEntity entity);
    List<QuantityMeasurementEntity> GetAll();
    List<QuantityMeasurementEntity> GetByOperation(string operation);
    List<QuantityMeasurementEntity> GetByType(string type);
    int GetTotalCount();
    void DeleteAll();
}
