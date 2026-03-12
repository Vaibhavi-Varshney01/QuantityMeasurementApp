using System;

namespace QuantityMeasurementModel.Models
{
    public class Quantity<U> where U : Enum
    {
        public double Value { get; set; }
        public U Unit { get; set; }

        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Quantity value must be a finite number");

            if (unit == null || !Enum.IsDefined(typeof(U), unit))
                throw new ArgumentException("Invalid Unit");

            Value = value;
            Unit = unit;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Quantity<U>))
                return false;

            Quantity<U> other = (Quantity<U>)obj;

            if (Unit.GetType() != other.Unit.GetType())
                throw new ArgumentException("Quantities must belong to same measurement category");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            return Math.Abs(base1 - base2) < 0.01;
        }

        public override int GetHashCode()
        {
            if (Unit is TemperatureUnit t)
            {
                double baseVal = ConvertToBase(Value, Unit);
                return baseVal.GetHashCode();
            }
            return HashCode.Combine(Value, Unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Unit is TemperatureUnit || other.Unit is TemperatureUnit)
                throw new QuantityMeasurementModel.Exceptions.QuantityMeasurementException("Temperature does not support addition");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double resultBase = base1 + base2;
            double finalValue = ConvertFromBase(resultBase, targetUnit);

            int precision = 2;
            if (targetUnit is VolumeUnit)
                precision = 3;
            else if (targetUnit is LengthUnit l && l == LengthUnit.Yard)
                precision = 3;
            return new Quantity<U>(Math.Round(finalValue, precision), targetUnit);
        }

        // Convenience overloads expected by older tests/consumers
        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, Unit);
        }

        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, Unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Unit is TemperatureUnit || other.Unit is TemperatureUnit)
                throw new QuantityMeasurementModel.Exceptions.QuantityMeasurementException("Temperature does not support subtraction");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double resultBase = base1 - base2;
            double finalValue = ConvertFromBase(resultBase, targetUnit);

            int precision = 2;
            if (targetUnit is VolumeUnit)
                precision = 3;
            else if (targetUnit is LengthUnit l && l == LengthUnit.Yard)
                precision = 3;
            return new Quantity<U>(Math.Round(finalValue, precision), targetUnit);
        }

        public double Divide(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Unit is TemperatureUnit || other.Unit is TemperatureUnit)
                throw new QuantityMeasurementModel.Exceptions.QuantityMeasurementException("Temperature does not support division");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            if (Math.Abs(base2) < 1e-10)
                throw new DivideByZeroException("Cannot divide by zero quantity");

            return base1 / base2;
        }

        public double ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(Value, Unit);
            return ConvertFromBase(baseValue, targetUnit);
        }

        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit l)
                return l switch
                {
                    LengthUnit.Feet => value,
                    LengthUnit.Yard => value * 3,
                    LengthUnit.Cm => value / 30.48,
                    LengthUnit.Inch => value / 12,
                    _ => throw new Exception("Invalid LengthUnit")
                };

            if (unit is WeightUnit w)
                return w switch
                {
                    WeightUnit.Kilogram => value,
                    WeightUnit.Gram => value / 1000,
                    WeightUnit.Pound => value * 0.453592,
                    WeightUnit.Tonne => value * 1000,
                    _ => throw new Exception("Invalid WeightUnit")
                };

            if (unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => value,
                    VolumeUnit.Millilitre => value / 1000,
                    VolumeUnit.Gallon => value * 3.78541,
                    _ => throw new Exception("Invalid VolumeUnit")
                };

            if (unit is TemperatureUnit t)
                return t switch
                {
                    TemperatureUnit.Celsius => value,
                    TemperatureUnit.Fahrenheit => (value - 32) * 5 / 9,
                    TemperatureUnit.Kelvin => value - 273.15,
                    _ => throw new Exception("Invalid TemperatureUnit")
                };

            throw new Exception("Unsupported unit type");
        }

        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthUnit l)
                return l switch
                {
                    LengthUnit.Feet => baseValue,
                    LengthUnit.Yard => baseValue / 3,
                    LengthUnit.Cm => baseValue * 30.48,
                    LengthUnit.Inch => baseValue * 12,
                    _ => throw new Exception("Invalid LengthUnit")
                };

            if (unit is WeightUnit w)
                return w switch
                {
                    WeightUnit.Kilogram => baseValue,
                    WeightUnit.Gram => baseValue * 1000,
                    WeightUnit.Pound => baseValue / 0.453592,
                    WeightUnit.Tonne => baseValue / 1000,
                    _ => throw new Exception("Invalid WeightUnit")
                };

            if (unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => baseValue,
                    VolumeUnit.Millilitre => baseValue * 1000,
                    VolumeUnit.Gallon => baseValue / 3.78541,
                    _ => throw new Exception("Invalid VolumeUnit")
                };

            if (unit is TemperatureUnit t)
                return t switch
                {
                    TemperatureUnit.Celsius => baseValue,
                    TemperatureUnit.Fahrenheit => (baseValue * 9 / 5) + 32,
                    TemperatureUnit.Kelvin => baseValue + 273.15,
                    _ => throw new Exception("Invalid TemperatureUnit")
                };

            throw new Exception("Unsupported unit type");
        }
    }
}
