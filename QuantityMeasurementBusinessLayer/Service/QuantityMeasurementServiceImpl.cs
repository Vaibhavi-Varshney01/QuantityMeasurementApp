using QuantityMeasurementModel.DTO;
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

        public QuantityMeasurementDTO Compare(QuantityDTO q1, QuantityDTO q2)
        {
            bool equal = Math.Abs(q1.Value - q2.Value) < 1e-9 && string.Equals(q1.Unit, q2.Unit, StringComparison.OrdinalIgnoreCase);

            _repository.Save(new QuantityMeasurementEntity(
                operand1: $"{q1.Value} {q1.Unit}",
                operand2: $"{q2.Value} {q2.Unit}",
                operationType: "COMPARE",
                result: equal.ToString(),
                measurementType: q1.MeasurementType));

            return new QuantityMeasurementDTO
            {
                ThisValue = q1.Value,
                ThisUnit = q1.Unit,
                ThisMeasurementType = q1.MeasurementType,
                ThatValue = q2.Value,
                ThatUnit = q2.Unit,
                ThatMeasurementType = q2.MeasurementType,
                Operation = "COMPARE",
                ResultString = equal.ToString(),
                IsError = false
            };
        }

        public QuantityMeasurementDTO Convert(QuantityDTO q, string targetUnit)
        {
            _repository.Save(new QuantityMeasurementEntity(
                operand1: $"{q.Value} {q.Unit}",
                operationType: "CONVERT",
                result: $"{q.Value} {targetUnit}",
                measurementType: q.MeasurementType));

            return new QuantityMeasurementDTO
            {
                ThisValue = q.Value,
                ThisUnit = q.Unit,
                ThisMeasurementType = q.MeasurementType,
                Operation = "CONVERT",
                ResultString = $"{q.Value} {targetUnit}",
                IsError = false
            };
        }

        public QuantityMeasurementDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            double result = q1.Value + q2.Value;
            _repository.Save(new QuantityMeasurementEntity(
                operand1: $"{q1.Value} {q1.Unit}",
                operand2: $"{q2.Value} {q2.Unit}",
                operationType: "ADD",
                result: $"{result} {q1.Unit}",
                measurementType: q1.MeasurementType));

            return new QuantityMeasurementDTO
            {
                ThisValue = q1.Value,
                ThisUnit = q1.Unit,
                ThisMeasurementType = q1.MeasurementType,
                ThatValue = q2.Value,
                ThatUnit = q2.Unit,
                ThatMeasurementType = q2.MeasurementType,
                Operation = "ADD",
                ResultValue = result,
                ResultUnit = q1.Unit,
                ResultMeasurementType = q1.MeasurementType,
                IsError = false
            };
        }

        public QuantityMeasurementDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            double result = q1.Value - q2.Value;
            _repository.Save(new QuantityMeasurementEntity(
                operand1: $"{q1.Value} {q1.Unit}",
                operand2: $"{q2.Value} {q2.Unit}",
                operationType: "SUBTRACT",
                result: $"{result} {q1.Unit}",
                measurementType: q1.MeasurementType));

            return new QuantityMeasurementDTO
            {
                ThisValue = q1.Value,
                ThisUnit = q1.Unit,
                ThisMeasurementType = q1.MeasurementType,
                ThatValue = q2.Value,
                ThatUnit = q2.Unit,
                ThatMeasurementType = q2.MeasurementType,
                Operation = "SUBTRACT",
                ResultValue = result,
                ResultUnit = q1.Unit,
                ResultMeasurementType = q1.MeasurementType,
                IsError = false
            };
        }

        public QuantityMeasurementDTO Divide(QuantityDTO q1, QuantityDTO q2)
        {
            if (Math.Abs(q2.Value) < 1e-12)
            {
                _repository.Save(new QuantityMeasurementEntity("Division by zero"));
                return new QuantityMeasurementDTO
                {
                    Operation = "DIVIDE",
                    ErrorMessage = "Division by zero",
                    IsError = true
                };
            }

            double result = q1.Value / q2.Value;
            _repository.Save(new QuantityMeasurementEntity(
                operand1: $"{q1.Value} {q1.Unit}",
                operand2: $"{q2.Value} {q2.Unit}",
                operationType: "DIVIDE",
                result: result.ToString(),
                measurementType: q1.MeasurementType));

            return new QuantityMeasurementDTO
            {
                ThisValue = q1.Value,
                ThisUnit = q1.Unit,
                ThisMeasurementType = q1.MeasurementType,
                ThatValue = q2.Value,
                ThatUnit = q2.Unit,
                ThatMeasurementType = q2.MeasurementType,
                Operation = "DIVIDE",
                ResultValue = result,
                IsError = false
            };
        }

        public List<QuantityMeasurementDTO> GetHistoryByOperation(string operation)
        {
            var entities = _repository.GetMeasurementsByOperation(operation);
            return entities.Select(e => new QuantityMeasurementDTO
            {
                Operation = e.OperationType,
                ThisMeasurementType = e.MeasurementType,
                ResultString = e.Result,
                ErrorMessage = e.ErrorMessage,
                IsError = e.HasError
            }).ToList();
        }

        public List<QuantityMeasurementDTO> GetHistoryByType(string type)
        {
            var entities = _repository.GetMeasurementsByType(type);
            return entities.Select(e => new QuantityMeasurementDTO
            {
                Operation = e.OperationType,
                ThisMeasurementType = e.MeasurementType,
                ResultString = e.Result,
                ErrorMessage = e.ErrorMessage,
                IsError = e.HasError
            }).ToList();
        }

        public List<QuantityMeasurementDTO> GetErrorHistory()
        {
            var entities = _repository.GetAllMeasurements().Where(e => e.HasError).ToList();
            return entities.Select(e => new QuantityMeasurementDTO
            {
                Operation = e.OperationType,
                ErrorMessage = e.ErrorMessage,
                IsError = true
            }).ToList();
        }

        public int CountByOperation(string operation)
            => _repository.GetMeasurementsByOperation(operation).Count;
    }
}
