// using QuantityMeasurementApp.Interfaces;

namespace QuantityMeasurementModel.Models
{
    public enum LengthUnit
    {
        Inch = 0,
        Feet = 1,
        Yard = 2,
        Cm = 3,

        // Synonyms to keep older code/tests compiling
        Inches = Inch,
        Yards = Yard,
        Centimeters = Cm,
        // Legacy uppercase names
        FEET = Feet,
        INCHES = Inch,
        YARD = Yard,
        CM = Cm
    }

    public static class LengthUnitExtensions
    {
        // conversion factors relative to the base unit Feet
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => 1.0,
                LengthUnit.Yard => 3.0,
                LengthUnit.Inch => 1.0 / 12.0,
                LengthUnit.Cm => 1.0 / 30.48, // feet per centimetre
                _ => throw new ArgumentException("Invalid Unit")
            };
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString().ToUpperInvariant();
        }
    }
}
