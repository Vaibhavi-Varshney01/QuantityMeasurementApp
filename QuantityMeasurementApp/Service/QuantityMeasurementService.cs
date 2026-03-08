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

            double val1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double val2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            return Math.Abs(val1 - val2) < 0.0001;
        }

        // UC5
        public double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double baseValue = source.ConvertToBaseUnit(value);

            return target.ConvertFromBaseUnit(baseValue);
        }

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

            double val1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double val2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            double sumBase = val1 + val2;

            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(result, targetUnit);
        }

        // -------------------------------
        // UC9 : Weight Measurement
        // -------------------------------

        // Compare Weight
        public bool AreWeightEqual(QuantityWeight w1, QuantityWeight w2)
        {
            if (w1 == null || w2 == null) return false;

            double val1 = w1.Unit.ConvertToBaseUnit(w1.Value);
            double val2 = w2.Unit.ConvertToBaseUnit(w2.Value);

            return Math.Abs(val1 - val2) < 0.0001;
        }

        // Convert Weight
        public double ConvertWeight(double value, WeightUnit source, WeightUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double baseValue = source.ConvertToBaseUnit(value);

            return target.ConvertFromBaseUnit(baseValue);
        }

        // Add Weight
        public QuantityWeight AddWeight(QuantityWeight w1, QuantityWeight w2)
        {
            if (w1 == null || w2 == null)
                throw new ArgumentException("Weight operands cannot be null");

            return AddWeight(w1, w2, w1.Unit);
        }

        // Add Weight with Target Unit
        public QuantityWeight AddWeight(QuantityWeight w1, QuantityWeight w2, WeightUnit targetUnit)
        {
            if (w1 == null || w2 == null)
                throw new ArgumentException("Weight cannot be null");

            double val1 = w1.Unit.ConvertToBaseUnit(w1.Value);
            double val2 = w2.Unit.ConvertToBaseUnit(w2.Value);

            double sumBase = val1 + val2;

            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(result, targetUnit);
        }
    }
}