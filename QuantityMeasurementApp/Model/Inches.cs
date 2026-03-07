namespace QuantityMeasurementApp.Model
{
    public class Inches
    {
        public double Value { get; }

        public Inches(double value)
        {
            Value = value;
        }

        // Null-safe Equals method
        public bool Equals(Inches other)
        {
            if (other == null) return false; // <-- null check added
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Inches); // call the null-safe method
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}