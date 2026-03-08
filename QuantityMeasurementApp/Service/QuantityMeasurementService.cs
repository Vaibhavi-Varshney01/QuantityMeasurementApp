using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        // Feet equality check
        public bool AreFeetEqual(Feet f1, Feet f2)
        {
            if (f1 == null || f2 == null) return false;
            return f1.Equals(f2);
        }

        // Inches equality check
        public bool AreInchesEqual(Inches i1, Inches i2)
        {
            if (i1 == null || i2 == null) return false;
            return i1.Equals(i2);
        }

        // Validate numeric Feet input from string
        public bool ValidateFeetInput(string input)
        {
            return double.TryParse(input, out _);
        }

        // Validate numeric Inches input from string
        public bool ValidateInchesInput(string input)
        {
            return double.TryParse(input, out _);
        }

        // Optional: Feet vs Inches conversion equality
        public bool AreFeetEqual(Feet f, Inches i)
        {
            if (f == null || i == null) return false;
            return f.Value * 12 == i.Value;
        }
    }
}