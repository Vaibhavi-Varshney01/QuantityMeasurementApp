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

        private double ConvertToFeet(double value, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12.0,
                LengthUnit.Yard => value * 3.0,
                LengthUnit.Cm => value * 0.393701 / 12.0, // cm -> inch -> feet
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}