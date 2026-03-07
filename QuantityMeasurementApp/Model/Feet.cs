using System;

namespace QuantityMeasurementApp.Model
{
    public class Feet
    {
        public double Value { get; }

        public Feet(double value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;

            Feet other = (Feet)obj;
            return Math.Abs(Value - other.Value) < 0.0001; // floating-point safe comparison
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}