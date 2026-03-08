using QuantityMeasurementApp.Unit;

namespace QuantityMeasurementApp.Unit
{
    public enum WeightUnit
    {
        GRAM,
        KILOGRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.GRAM => 1.0,
                WeightUnit.KILOGRAM => 1000.0,
                WeightUnit.POUND => 453.592,
                _ => throw new ArgumentException("Unknown WeightUnit")
            };
        }

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
            => value * unit.GetConversionFactor();

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
            => baseValue / unit.GetConversionFactor();

        public static string GetUnitName(this WeightUnit unit) => unit.ToString();
    }
}