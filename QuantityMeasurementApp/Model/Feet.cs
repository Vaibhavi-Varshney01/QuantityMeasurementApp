namespace QuantityMeasurementApp.Model
{
    public class Feet
    {
        public double Value { get; }

        public Feet(double value)
        {
            Value = value;
        }

        // Null-safe Equals method
        public bool Equals(Feet other)
        {
            if (other == null) return false; // <-- null check added
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Feet); // call the null-safe method
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}