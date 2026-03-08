using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        // UC1 + UC2
        public bool AreFeetEqual(Feet f1, Feet f2) => f1 != null && f2 != null && f1.Equals(f2);
        public bool AreInchesEqual(Inches i1, Inches i2) => i1 != null && i2 != null && i1.Equals(i2);

        // UC3 + UC4
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
                LengthUnit.Cm => value * 0.0328084,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        // UC5
        public double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (!Enum.IsDefined(typeof(LengthUnit), source) || !Enum.IsDefined(typeof(LengthUnit), target))
                throw new ArgumentException("Unsupported unit");

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double valueInFeet = ConvertToFeet(value, source);

            return target switch
            {
                LengthUnit.Feet => valueInFeet,
                LengthUnit.Inch => valueInFeet * 12.0,
                LengthUnit.Yard => valueInFeet / 3.0,
                LengthUnit.Cm => valueInFeet * 30.48,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

      
// UC6
// UC6
public QuantityLength Add(QuantityLength q1, QuantityLength q2)
{
    if (q1 == null || q2 == null)
        throw new ArgumentException("Operands cannot be null");

    return Add(q1, q2, q1.Unit);
}

// UC7
public QuantityLength Add(QuantityLength q1, QuantityLength q2, LengthUnit targetUnit)
{
    if (q1 == null || q2 == null)
        throw new ArgumentException("Quantity cannot be null");

    double val1 = ConvertToFeet(q1.Value, q1.Unit);
    double val2 = ConvertToFeet(q2.Value, q2.Unit);

    double sumFeet = val1 + val2;

    double result = Convert(sumFeet, LengthUnit.Feet, targetUnit);

    return new QuantityLength(result, targetUnit);
} 
  }
}