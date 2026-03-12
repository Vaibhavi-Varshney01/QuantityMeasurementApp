using System;
using QuantityMeasurementModel.Models;

namespace QuantityMeasurementModel.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public object? Operand1 { get; }
        public object? Operand2 { get; }
        public string OperationType { get; }
        public object? Result { get; }
        public string? ErrorMessage { get; }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        // Single-operand constructor (e.g., convert)
        public QuantityMeasurementEntity(object operand1, string operationType, object result)
        {
            Operand1 = operand1 ?? throw new ArgumentNullException(nameof(operand1));
            OperationType = operationType ?? throw new ArgumentNullException(nameof(operationType));
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }

        // Binary-operand constructor (e.g., add/subtract)
        public QuantityMeasurementEntity(object operand1, object operand2, string operationType, object result)
        {
            Operand1 = operand1 ?? throw new ArgumentNullException(nameof(operand1));
            Operand2 = operand2 ?? throw new ArgumentNullException(nameof(operand2));
            OperationType = operationType ?? throw new ArgumentNullException(nameof(operationType));
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }

        // Error constructor
        public QuantityMeasurementEntity(string errorMessage)
        {
            ErrorMessage = errorMessage ?? "Unknown error";
            OperationType = "ERROR";
        }
    }
}
