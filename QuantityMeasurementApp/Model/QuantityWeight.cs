namespace QuantityMeasurementApp.Model
{
    public class QuantityWeight
    {
        public double Value { get; set; }

        public QuantityWeight(double value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != this.GetType()) return false;

            var other = (QuantityWeight)obj;
            return Value == other.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}