using System;

namespace QuantityMeasurementApp.Model
{
    public class QuantityLength
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        // Convert to Feet as base unit for comparison
        private double ToFeet()
        {
            return Unit switch
            {
                LengthUnit.Feet => Value,
                LengthUnit.Inch => Value / 12,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(QuantityLength))
                return false;

            QuantityLength other = (QuantityLength)obj;
            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001; // tolerance for floating point
        }

        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }
    }
}