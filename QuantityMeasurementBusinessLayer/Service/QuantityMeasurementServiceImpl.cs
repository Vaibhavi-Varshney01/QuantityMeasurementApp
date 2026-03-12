using QuantityMeasurementBusinessLayer.DTO;
using QuantityMeasurementModel;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository;

namespace QuantityMeasurementBusinessLayer
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        public QuantityDTO Compare(QuantityDTO q1, QuantityDTO q2)
        {
            var result = new QuantityDTO { Value = q1.Value - q2.Value }; // dummy logic
            var entity = new QuantityMeasurementEntity(
                new QuantityModel<object> { Value = q1.Value, Unit = q1.Unit! },
                new QuantityModel<object> { Value = q2.Value, Unit = q2.Unit! },
                "Compare",
                new QuantityModel<object> { Value = result.Value, Unit = q1.Unit! }
            );
            _repository.Save(entity);
            return result;
        }

        public QuantityDTO Convert(QuantityDTO q, string targetUnit)
        {
            return new QuantityDTO { Value = q.Value, Unit = targetUnit };
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            return new QuantityDTO { Value = q1.Value + q2.Value };
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            return new QuantityDTO { Value = q1.Value - q2.Value };
        }

        public QuantityDTO Divide(QuantityDTO q1, QuantityDTO q2)
        {
            return new QuantityDTO { Value = q1.Value / q2.Value };
        }
    }
}
