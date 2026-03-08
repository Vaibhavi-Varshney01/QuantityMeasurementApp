using QuantityMeasurementApp.Unit;

namespace QuantityMeasurementApp.Unit
{
    public enum LengthUnit : int
    {
        FEET = 1,
        INCHES = 12,
        YARDS = 36,
        CENTIMETERS = 30 // just example factor to inches
    }

    public static class LengthUnitExtensions
    {
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => 12.0,
                LengthUnit.INCHES => 1.0,
                LengthUnit.YARDS => 36.0,
                LengthUnit.CENTIMETERS => 0.393701,
                _ => throw new ArgumentException("Unknown LengthUnit")
            };
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
            => value * unit.GetConversionFactor();

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
            => baseValue / unit.GetConversionFactor();

        public static string GetUnitName(this LengthUnit unit) => unit.ToString();
    }
}