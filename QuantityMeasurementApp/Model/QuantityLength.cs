using System;

namespace QuantityMeasurementApp.Model
{
    public class QuantityLength
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite", nameof(value));

            Value = value;
            Unit = unit;
        }

        // UC1–UC4 Equality
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(QuantityLength))
                return false;

            var other = (QuantityLength)obj;

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return Unit.ConvertToBaseUnit(Value).GetHashCode();
        }

        // UC6
        public QuantityLength Add(QuantityLength other)
        {
            return Add(other, this.Unit);
        }

        // UC7
        public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double sumBase = base1 + base2;

            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(result, targetUnit);
        }

        // UC5 Conversion
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = Unit.ConvertToBaseUnit(Value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityLength(converted, targetUnit);
        }

        public override string ToString()
        {
            return $"Quantity({Value:F3}, {Unit})";
        }
    }
}