using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        // UC1 + UC2
        public bool AreFeetEqual(Feet f1, Feet f2) => f1 != null && f2 != null && f1.Equals(f2);
        public bool AreInchesEqual(Inches i1, Inches i2) => i1 != null && i2 != null && i1.Equals(i2);
        public bool AreFeetEqual(Feet f, Inches i) => f != null && i != null && Math.Abs(f.Value * 12 - i.Value) < 0.0001;

        public bool ValidateFeetInput(string input) => double.TryParse(input, out _);
        public bool ValidateInchesInput(string input) => double.TryParse(input, out _);

        // UC3 + UC4
        public void ValidateQuantityLength(double value, LengthUnit unit)
        {
            if (value < 0) throw new ArgumentException("Measurement cannot be negative");
            if (!Enum.IsDefined(typeof(LengthUnit), unit)) throw new ArgumentException("Unsupported unit");
        }

        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null) return false;
            double val1 = ConvertToFeet(q1.Value, q1.Unit);
            double val2 = ConvertToFeet(q2.Value, q2.Unit);
            return Math.Abs(val1 - val2) < 0.0001;
        }

        private double ConvertToFeet(double value, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12.0,
                LengthUnit.Yard => value * 3.0,
                LengthUnit.Cm => value * 0.393701 / 12.0,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}