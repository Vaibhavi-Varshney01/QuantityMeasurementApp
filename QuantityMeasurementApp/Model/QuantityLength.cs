using System;

namespace QuantityMeasurementApp.Model
{
    public class QuantityLength
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite", nameof(value));

            Value = value;
            Unit = unit;
        }

        // UC1–UC4: Equality
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(QuantityLength))
                return false;

            var other = (QuantityLength)obj;
            return Math.Abs(ConvertToFeet(Value, Unit) - ConvertToFeet(other.Value, other.Unit)) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet(Value, Unit).GetHashCode();
        }

        // UC6: Add defaulting to first operand's unit
        public QuantityLength Add(QuantityLength other)
        {
            return Add(other, this.Unit);
        }

        // UC7: Add with explicit target unit
        public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (targetUnit == null) throw new ArgumentNullException(nameof(targetUnit));

            double baseValue1 = ConvertToFeet(Value, Unit);
            double baseValue2 = ConvertToFeet(other.Value, other.Unit);

            double sumInBase = baseValue1 + baseValue2;

            double sumInTarget = ConvertFromFeet(sumInBase, targetUnit);

            return new QuantityLength(sumInTarget, targetUnit);
        }

        private double ConvertToFeet(double value, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12.0,
                LengthUnit.Yard => value * 3.0,
                LengthUnit.Cm => value * 0.0328084,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        private double ConvertFromFeet(double valueInFeet, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => valueInFeet,
                LengthUnit.Inch => valueInFeet * 12.0,
                LengthUnit.Yard => valueInFeet / 3.0,
                LengthUnit.Cm => valueInFeet / 0.0328084,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        public override string ToString()
        {
            return $"Quantity({Value:F3}, {Unit})";
        }
    }
}