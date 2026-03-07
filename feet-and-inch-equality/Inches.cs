using System;

namespace feet_and_inches_equality
{
    public class Inches
    {
        private double value;

        public Inches(double value)
        {
            if (value < 0)
                throw new InvalidMeasurementException("Inches value cannot be negative");
            this.value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Inches))
                return false;

            Inches other = (Inches)obj;
            return this.value == other.value;
        }

        public override int GetHashCode() => value.GetHashCode();
    }
}