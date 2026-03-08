namespace QuantityMeasurementApp.Model
{
    public class Feet
    {
        public double Value { get; set; }

        public Feet(double value)
        {
            Value = value;
        }

        // Null-safe Equals override
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != this.GetType()) return false;

            var other = (Feet)obj;
            return Value == other.Value;
        }

        // Keep only one GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}