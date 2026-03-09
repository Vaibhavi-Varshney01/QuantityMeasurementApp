using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                return false;

            return q1.Equals(q2);
        }

        public bool AreFeetEqual(Feet f1, Feet f2)
        {
            if (f1 == null || f2 == null)
                return false;

            return f1.Equals(f2);
        }

        public bool AreInchesEqual(Inches i1, Inches i2)
        {
            if (i1 == null || i2 == null)
                return false;

            return i1.Equals(i2);
        }

        public QuantityLength Add(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            return q1.Add(q2);
        }

        public QuantityLength Add(QuantityLength q1, QuantityLength q2, LengthUnit targetUnit)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            return q1.Add(q2, targetUnit);
        }

        public double Convert(double value, LengthUnit from, LengthUnit to)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite");
            if (!Enum.IsDefined(typeof(LengthUnit), from) || !Enum.IsDefined(typeof(LengthUnit), to))
                throw new ArgumentException("Invalid length unit");

            double baseValue = from.ConvertToBaseUnit(value);
            return to.ConvertFromBaseUnit(baseValue);
        }

        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        // Centralized helper for all arithmetic operations
        private double PerformBaseArithmetic<U>(Quantity<U> q1, Quantity<U> q2, ArithmeticOperation operation) where U : Enum
        {
            ValidateUnitOperationSupport(q1.Unit, operation);
            ValidateUnitOperationSupport(q2.Unit, operation);
            ValidateArithmeticOperands(q1, q2, operation != ArithmeticOperation.DIVIDE);

            double base1 = ConvertToBase(q1);
            double base2 = ConvertToBase(q2);

            double result = operation switch
            {
                ArithmeticOperation.ADD => base1 + base2,
                ArithmeticOperation.SUBTRACT => base1 - base2,
                ArithmeticOperation.DIVIDE => Math.Abs(base2) < 1e-10
                    ? throw new ArithmeticException("Cannot divide by zero quantity")
                    : base1 / base2,
                _ => throw new Exception("Invalid arithmetic operation")
            };

            return result;
        }

        private static void ValidateUnitOperationSupport<U>(U unit, ArithmeticOperation operation) where U : Enum
        {
            if (unit is TemperatureUnit)
            {
                throw new NotSupportedException(
                    $"Temperature does not support {operation.ToString().ToLowerInvariant()} operation.");
            }
        }

        private void ValidateArithmeticOperands<U>(Quantity<U> q1, Quantity<U> q2, bool requireTargetUnit) where U : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (q1.Unit.GetType() != q2.Unit.GetType())
                throw new ArgumentException("Quantities must be of the same measurement category");

            if (double.IsNaN(q1.Value) || double.IsInfinity(q1.Value) ||
                double.IsNaN(q2.Value) || double.IsInfinity(q2.Value))
                throw new ArgumentException("Quantity values must be finite numbers");
        }

        // Public Add
        public Quantity<U> Add<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit = default!) where U : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            U target = EqualityComparer<U>.Default.Equals(targetUnit, default) ? q1.Unit : targetUnit;
            double resultBase = PerformBaseArithmetic(q1, q2, ArithmeticOperation.ADD);
            double finalValue = ConvertFromBase(resultBase, target);
            return new Quantity<U>(Math.Round(finalValue, 2), target);
        }

        // Public Subtract
        public Quantity<U> Subtract<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit = default!) where U : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            U target = EqualityComparer<U>.Default.Equals(targetUnit, default) ? q1.Unit : targetUnit;
            double resultBase = PerformBaseArithmetic(q1, q2, ArithmeticOperation.SUBTRACT);
            double finalValue = ConvertFromBase(resultBase, target);
            return new Quantity<U>(Math.Round(finalValue, 2), target);
        }

        // Public Divide
        public double Divide<U>(Quantity<U> q1, Quantity<U> q2) where U : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            return PerformBaseArithmetic(q1, q2, ArithmeticOperation.DIVIDE);
        }

        // Equality check
        public bool AreEqual<U>(Quantity<U> q1, Quantity<U> q2) where U : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (q1.Unit.GetType() != q2.Unit.GetType())
                throw new ArgumentException("Quantities must be of the same measurement category");

            double baseQ1 = ConvertToBase(q1);
            double baseQ2 = ConvertToBase(q2);
            return Math.Abs(baseQ1 - baseQ2) < 1e-2;
        }

        public Quantity<U> GenericAdd<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit = default!) where U : Enum
        {
            return Add(q1, q2, targetUnit);
        }

        public Quantity<U> GenericSubtract<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit = default!) where U : Enum
        {
            return Subtract(q1, q2, targetUnit);
        }

        public double GenericDivide<U>(Quantity<U> q1, Quantity<U> q2) where U : Enum
        {
            return Divide(q1, q2);
        }

        public bool GenericAreEqual<U>(Quantity<U> q1, Quantity<U> q2) where U : Enum
        {
            return AreEqual(q1, q2);
        }

        public Quantity<U> GenericConvert<U>(Quantity<U> quantity, U targetUnit) where U : Enum
        {
            if (quantity == null)
                throw new ArgumentException("Quantity cannot be null");

            double baseValue = ConvertToBase(quantity);
            double converted = ConvertFromBase(baseValue, targetUnit);
            return new Quantity<U>(Math.Round(converted, 2), targetUnit);
        }

        public Quantity<U1> GenericSubtractCross<U1, U2>(Quantity<U1> q1, Quantity<U2> q2, U1 targetUnit = default!) where U1 : Enum where U2 : Enum
        {
            throw new ArgumentException("Quantities must be of the same measurement category");
        }

        public double GenericDivideCross<U1, U2>(Quantity<U1> q1, Quantity<U2> q2) where U1 : Enum where U2 : Enum
        {
            throw new ArgumentException("Quantities must be of the same measurement category");
        }

        // Convert to base unit
        private double ConvertToBase<U>(Quantity<U> quantity) where U : Enum
        {
            if (quantity.Unit is LengthUnit l)
                return l switch
                {
                    LengthUnit.Feet => quantity.Value * 12,
                    LengthUnit.Yard => quantity.Value * 36,
                    LengthUnit.Cm => quantity.Value / 2.54,
                    LengthUnit.Inch => quantity.Value,
                    _ => throw new Exception("Invalid LengthUnit")
                };
            else if (quantity.Unit is WeightUnit w)
                return w switch
                {
                    WeightUnit.Kg => quantity.Value,
                    WeightUnit.Gm => quantity.Value / 1000,
                    WeightUnit.Lb => quantity.Value * 0.453592,
                    WeightUnit.Tonne => quantity.Value * 1000,
                    _ => throw new Exception("Invalid WeightUnit")
                };
            else if (quantity.Unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => quantity.Value,
                    VolumeUnit.Millilitre => quantity.Value / 1000,
                    VolumeUnit.Gallon => quantity.Value * 3.78541,
                    _ => throw new Exception("Invalid VolumeUnit")
                };
            else if (quantity.Unit is TemperatureUnit t)
                return t switch
                {
                    TemperatureUnit.Celsius => quantity.Value,
                    TemperatureUnit.Fahrenheit => (quantity.Value - 32.0) * 5.0 / 9.0,
                    TemperatureUnit.Kelvin => quantity.Value - 273.15,
                    _ => throw new Exception("Invalid TemperatureUnit")
                };
            else
                throw new Exception("Unsupported Unit Type");
        }

        // Convert from base unit
        private double ConvertFromBase<U>(double baseValue, U unit) where U : Enum
        {
            if (unit is LengthUnit l)
                return l switch
                {
                    LengthUnit.Feet => baseValue / 12,
                    LengthUnit.Yard => baseValue / 36,
                    LengthUnit.Cm => baseValue * 2.54,
                    LengthUnit.Inch => baseValue,
                    _ => throw new Exception("Invalid LengthUnit")
                };
            else if (unit is WeightUnit w)
                return w switch
                {
                    WeightUnit.Kg => baseValue,
                    WeightUnit.Gm => baseValue * 1000,
                    WeightUnit.Lb => baseValue / 0.453592,
                    WeightUnit.Tonne => baseValue / 1000,
                    _ => throw new Exception("Invalid WeightUnit")
                };
            else if (unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => baseValue,
                    VolumeUnit.Millilitre => baseValue * 1000,
                    VolumeUnit.Gallon => baseValue / 3.78541,
                    _ => throw new Exception("Invalid VolumeUnit")
                };
            else if (unit is TemperatureUnit t)
                return t switch
                {
                    TemperatureUnit.Celsius => baseValue,
                    TemperatureUnit.Fahrenheit => (baseValue * 9.0 / 5.0) + 32.0,
                    TemperatureUnit.Kelvin => baseValue + 273.15,
                    _ => throw new Exception("Invalid TemperatureUnit")
                };
            else
                throw new Exception("Unsupported Unit Type");
        }
    }
}
