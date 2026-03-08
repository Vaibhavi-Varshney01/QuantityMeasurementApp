using System;

namespace QuantityMeasurementApp.Model
{
    public class Quantity<U>
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            Value = value;
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
        }

        // Subtraction (UC12)
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit = default)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (!typeof(U).Equals(other.Unit.GetType())) 
                throw new ArgumentException("Cannot subtract quantities of different categories");

            double baseThis = ConvertToBaseUnit(Value, Unit);
            double baseOther = ConvertToBaseUnit(other.Value, other.Unit);
            double resultBase = baseThis - baseOther;

            U target = targetUnit.Equals(default(U)) ? Unit : targetUnit;
            double finalValue = ConvertFromBaseUnit(resultBase, target);
            return new Quantity<U>(Math.Round(finalValue, 2), target);
        }

        // Division (UC12)
        public double Divide(Quantity<U> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (!typeof(U).Equals(other.Unit.GetType())) 
                throw new ArgumentException("Cannot divide quantities of different categories");

            double baseThis = ConvertToBaseUnit(Value, Unit);
            double baseOther = ConvertToBaseUnit(other.Value, other.Unit);
            if (Math.Abs(baseOther) < 1e-10) throw new DivideByZeroException("Cannot divide by zero quantity");
            return baseThis / baseOther;
        }

        // Conversion helpers
        private double ConvertToBaseUnit(double value, U unit)
        {
            if (unit is LengthUnit l)
                return l switch
                {
                    LengthUnit.Feet => value * 12,
                    LengthUnit.Yard => value * 36,
                    LengthUnit.Cm => value / 2.54,
                    LengthUnit.Inch => value,
                    _ => throw new Exception("Invalid LengthUnit")
                };
            else if (unit is WeightUnit w)
                return w switch
                {
                    WeightUnit.Kg => value,
                    WeightUnit.Gm => value / 1000,
                    WeightUnit.Lb => value * 0.453592,
                    _ => throw new Exception("Invalid WeightUnit")
                };
            else if (unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => value,
                    VolumeUnit.Millilitre => value / 1000,
                    _ => throw new Exception("Invalid VolumeUnit")
                };
            else
                throw new Exception("Unsupported Unit Type");
        }

        private double ConvertFromBaseUnit(double baseValue, U unit)
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
                    _ => throw new Exception("Invalid WeightUnit")
                };
            else if (unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => baseValue,
                    VolumeUnit.Millilitre => baseValue * 1000,
                    _ => throw new Exception("Invalid VolumeUnit")
                };
            else
                throw new Exception("Unsupported Unit Type");
        }
    }
}