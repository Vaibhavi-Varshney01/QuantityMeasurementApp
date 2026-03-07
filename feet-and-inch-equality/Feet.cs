using System;

namespace feet_and_inches_equality
{
    public class Feet
    {
        private double value;

        public Feet(double value)
        {
            if (value < 0)
                throw new InvalidMeasurementException("Feet value cannot be negative");
            this.value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Feet))
                return false;

            Feet other = (Feet)obj;
            return this.value == other.value;
        }

        public override int GetHashCode() => value.GetHashCode();
    }
}