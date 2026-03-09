using System;

namespace QuantityMeasurementApp.Model
{
    public enum WeightUnit
    {
        Gram = 1,
        GRAM = Gram,
        Gm = Gram,
        Kilogram = 2,
        KILOGRAM = Kilogram,
        Kg = Kilogram,
        Pound = 3,
        Lb = Pound,
        Tonne = 4
    }

    public static class WeightUnitExtensions
    {
        private const double PoundsPerKilogram = 2.20462;
        private static readonly SupportsArithmetic SupportsArithmetic = () => true;

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.Gram => value,
                WeightUnit.Kilogram => value * 1000.0,
                WeightUnit.Pound => value / PoundsPerKilogram * 1000.0,
                WeightUnit.Tonne => value * 1_000_000.0,
                _ => throw new ArgumentException("Invalid WeightUnit")
            };
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return unit switch
            {
                WeightUnit.Gram => baseValue,
                WeightUnit.Kilogram => baseValue / 1000.0,
                WeightUnit.Pound => (baseValue / 1000.0) * PoundsPerKilogram,
                WeightUnit.Tonne => baseValue / 1_000_000.0,
                _ => throw new ArgumentException("Invalid WeightUnit")
            };
        }

        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString().ToUpperInvariant();
        }

        public static bool SupportsArithmeticOperation(this WeightUnit unit)
        {
            return SupportsArithmetic();
        }

        public static void ValidateOperationSupport(this WeightUnit unit, ArithmeticOperation operation)
        {
            
        }
    }
}
