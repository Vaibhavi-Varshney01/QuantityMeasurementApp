using System;
namespace feet_measurement_equality{
    public class Feet{
        public double Value { get; }
        public Feet(double value){
            if (value < 0)
            {
                throw new InvalidFeetException("Feet value cannot be negative");
            }

            Value = value;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Feet other)
                return false;

            return this.Value == other.Value;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}