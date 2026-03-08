namespace QuantityMeasurementApp.Model
{
    public class Inches
    {
        public double Value { get; set; }

        public Inches(double value)
        {
            Value = value;
        }

        // Proper Equals override
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != this.GetType()) return false;

            var other = (Inches)obj;
            return Value == other.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}