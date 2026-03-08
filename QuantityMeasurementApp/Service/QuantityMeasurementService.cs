using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        // ===== UC1 + UC2 =====
        public bool AreFeetEqual(Feet f1, Feet f2)
        {
            if (f1 == null || f2 == null) return false;
            return f1.Equals(f2);
        }

        public bool AreInchesEqual(Inches i1, Inches i2)
        {
            if (i1 == null || i2 == null) return false;
            return i1.Equals(i2);
        }

        public bool AreFeetEqual(Feet f, Inches i)
        {
            if (f == null || i == null) return false;
            return Math.Abs(f.Value * 12 - i.Value) < 0.0001;
        }

        public bool ValidateFeetInput(string input)
        {
            return double.TryParse(input, out _);
        }

        public bool ValidateInchesInput(string input)
        {
            return double.TryParse(input, out _);
        }

        // ===== UC3 - QuantityLength (Feet + Inches only) =====
        public void ValidateQuantityLength(double value, LengthUnit unit)
        {
            if (value < 0) throw new ArgumentException("Measurement cannot be negative");
            if (!Enum.IsDefined(typeof(LengthUnit), unit))
                throw new ArgumentException("Unsupported unit type");
        }

        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null) return false;

            double value1InFeet = ConvertToFeet(q1.Value, q1.Unit);
            double value2InFeet = ConvertToFeet(q2.Value, q2.Unit);

            return Math.Abs(value1InFeet - value2InFeet) < 0.0001;
        }

        private double ConvertToFeet(double value, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12.0,
                _ => throw new ArgumentException("Unsupported unit for conversion")
            };
        }
    }
}