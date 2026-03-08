using System;

namespace QuantityMeasurementApp.Model
{
    public class Quantity<U> where U : Enum
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite number");
            Value = value;
            Unit = unit;
        }

        // --- Enum for arithmetic operations ---
        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        // --- Public Methods ---
        public Quantity<U> Add(Quantity<U> other) => Add(other, Unit);
        public Quantity<U> Add(Quantity<U> other, U targetUnit) =>
            new Quantity<U>(PerformBaseArithmetic(other, targetUnit, ArithmeticOperation.ADD), targetUnit);

        public Quantity<U> Subtract(Quantity<U> other) => Subtract(other, Unit);
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit) =>
            new Quantity<U>(PerformBaseArithmetic(other, targetUnit, ArithmeticOperation.SUBTRACT), targetUnit);

        public double Divide(Quantity<U> other) => PerformBaseArithmetic(other, default(U), ArithmeticOperation.DIVIDE);

        // --- Centralized Helper ---
        private double PerformBaseArithmetic(Quantity<U> other, U targetUnit, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other, targetUnit, operation != ArithmeticOperation.DIVIDE);

            double thisBase = ConvertToBaseUnit(Value, Unit);
            double otherBase = ConvertToBaseUnit(other.Value, other.Unit);
            double result = operation switch
            {
                ArithmeticOperation.ADD => thisBase + otherBase,
                ArithmeticOperation.SUBTRACT => thisBase - otherBase,
                ArithmeticOperation.DIVIDE =>
                    otherBase == 0 ? throw new DivideByZeroException("Cannot divide by zero") : thisBase / otherBase,
                _ => throw new NotSupportedException("Operation not supported")
            };

            if (operation != ArithmeticOperation.DIVIDE)
                result = ConvertFromBaseUnit(result, targetUnit);

            return RoundToTwoDecimals(result, operation);
        }

        // --- Validation Helper ---
        private void ValidateArithmeticOperands(Quantity<U> other, U targetUnit, bool targetRequired)
        {
            if (other == null) throw new ArgumentNullException(nameof(other), "Operand cannot be null");
            if (!Unit.GetType().Equals(other.Unit.GetType()))
                throw new ArgumentException("Cannot operate on different unit categories");
            if (double.IsNaN(Value) || double.IsInfinity(Value) || double.IsNaN(other.Value) || double.IsInfinity(other.Value))
                throw new ArgumentException("Values must be finite numbers");
            if (targetRequired && targetUnit == null)
                throw new ArgumentNullException(nameof(targetUnit), "Target unit cannot be null");
        }

        // --- Base Unit Conversion Helpers (stub for demonstration) ---
        private double ConvertToBaseUnit(double value, U unit)
        {
            // Convert to base unit logic
            return value; // Replace with real conversion logic per category
        }

        private double ConvertFromBaseUnit(double value, U unit)
        {
            // Convert from base unit logic
            return value; // Replace with real conversion logic per category
        }

        private double RoundToTwoDecimals(double value, ArithmeticOperation operation)
        {
            return operation == ArithmeticOperation.DIVIDE ? value : Math.Round(value, 2);
        }
    }
}