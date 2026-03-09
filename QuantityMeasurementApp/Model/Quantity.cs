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
            if (!Enum.IsDefined(typeof(U), unit))
                throw new ArgumentException("Unit must be a defined enum value", nameof(unit));
            Value = value;
            Unit = unit;
        }

        // --- Public Methods ---
        public Quantity<U> Add(Quantity<U> other) => Add(other, Unit);
        public Quantity<U> Add(Quantity<U> other, U targetUnit) =>
            new Quantity<U>(PerformBaseArithmetic(other, targetUnit, ArithmeticOperation.Add), targetUnit);

        public Quantity<U> Subtract(Quantity<U> other) => Subtract(other, Unit);
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit) =>
            new Quantity<U>(PerformBaseArithmetic(other, targetUnit, ArithmeticOperation.Subtract), targetUnit);

        public double Divide(Quantity<U> other) => PerformBaseArithmetic(other, default!, ArithmeticOperation.Divide);

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBaseUnit(Value, Unit);
            double converted = ConvertFromBaseUnit(baseValue, targetUnit);
            return new Quantity<U>(Math.Round(converted, 2), targetUnit);
        }

        // --- Centralized Helper ---
        private double PerformBaseArithmetic(Quantity<U> other, U targetUnit, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other, targetUnit, operation != ArithmeticOperation.Divide);

            ValidateUnitOperationSupport(Unit, operation);
            ValidateUnitOperationSupport(other.Unit, operation);

            double thisBase = ConvertToBaseUnit(Value, Unit);
            double otherBase = ConvertToBaseUnit(other.Value, other.Unit);
            double result = operation switch
            {
                ArithmeticOperation.Add => thisBase + otherBase,
                ArithmeticOperation.Subtract => thisBase - otherBase,
                ArithmeticOperation.Divide =>
                    otherBase == 0 ? throw new DivideByZeroException("Cannot divide by zero") : thisBase / otherBase,
                _ => throw new NotSupportedException("Operation not supported")
            };

            if (operation != ArithmeticOperation.Divide)
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
            if (unit is LengthUnit lengthUnit)
                return lengthUnit.ConvertToBaseUnit(value);

            if (unit is WeightUnit weightUnit)
                return weightUnit.ConvertToBaseUnit(value);

            if (unit is VolumeUnit volumeUnit)
                return volumeUnit.ConvertToBaseUnit(value);

            if (unit is TemperatureUnit temperatureUnit)
                return temperatureUnit.ConvertToBaseUnit(value);

            string unitName = unit.ToString().ToUpperInvariant();
            return unitName switch
            {
                "FEET" or "FOOT" => value,
                "INCH" or "INCHES" => value / 12.0,
                "YARD" or "YARDS" => value * 3.0,
                "CM" or "CENTIMETER" or "CENTIMETERS" => value / 30.48,
                "GRAM" or "GRAMS" => value,
                "KILOGRAM" or "KILOGRAMS" => value * 1000.0,
                "POUND" or "POUNDS" => value / 2.20462 * 1000.0,
                "LITRE" or "LITER" or "LITRES" or "LITERS" => value,
                "MILLILITRE" or "MILLILITER" or "MILLILITRES" or "MILLILITERS" => value / 1000.0,
                "GALLON" or "GALLONS" => value * 3.78541,
                "CELSIUS" => value,
                "FAHRENHEIT" => (value - 32.0) * 5.0 / 9.0,
                "KELVIN" => value - 273.15,
                _ => throw new NotSupportedException($"Unsupported unit type: {typeof(U).Name}")
            };
        }

        private double ConvertFromBaseUnit(double value, U unit)
        {
            if (unit is LengthUnit lengthUnit)
                return lengthUnit.ConvertFromBaseUnit(value);

            if (unit is WeightUnit weightUnit)
                return weightUnit.ConvertFromBaseUnit(value);

            if (unit is VolumeUnit volumeUnit)
                return volumeUnit.ConvertFromBaseUnit(value);

            if (unit is TemperatureUnit temperatureUnit)
                return temperatureUnit.ConvertFromBaseUnit(value);

            string unitName = unit.ToString().ToUpperInvariant();
            return unitName switch
            {
                "FEET" or "FOOT" => value,
                "INCH" or "INCHES" => value * 12.0,
                "YARD" or "YARDS" => value / 3.0,
                "CM" or "CENTIMETER" or "CENTIMETERS" => value * 30.48,
                "GRAM" or "GRAMS" => value,
                "KILOGRAM" or "KILOGRAMS" => value / 1000.0,
                "POUND" or "POUNDS" => (value / 1000.0) * 2.20462,
                "LITRE" or "LITER" or "LITRES" or "LITERS" => value,
                "MILLILITRE" or "MILLILITER" or "MILLILITRES" or "MILLILITERS" => value * 1000.0,
                "GALLON" or "GALLONS" => value / 3.78541,
                "CELSIUS" => value,
                "FAHRENHEIT" => (value * 9.0 / 5.0) + 32.0,
                "KELVIN" => value + 273.15,
                _ => throw new NotSupportedException($"Unsupported unit type: {typeof(U).Name}")
            };
        }

        private double RoundToTwoDecimals(double value, ArithmeticOperation operation)
        {
            return operation == ArithmeticOperation.Divide ? value : Math.Round(value, 2);
        }

        private static void ValidateUnitOperationSupport(U unit, ArithmeticOperation operation)
        {
            if (unit is TemperatureUnit temperatureUnit)
            {
                temperatureUnit.ValidateOperationSupport(operation);
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<U> other)
                return false;

            double thisBase = ConvertToBaseUnit(Value, Unit);
            double otherBase = ConvertToBaseUnit(other.Value, other.Unit);
            return Math.Abs(thisBase - otherBase) < 1e-2;
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit(Value, Unit).GetHashCode();
        }
    }
}
