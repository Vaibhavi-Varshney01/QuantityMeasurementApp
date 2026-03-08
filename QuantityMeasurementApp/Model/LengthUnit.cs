using System;

namespace QuantityMeasurementApp.Model
{
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yard,
        Cm
    }

    public static class LengthUnitExtensions
    {
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
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

        public static double ConvertFromBaseUnit(this LengthUnit unit, double valueInFeet)
        {
            return unit switch
            {
                LengthUnit.Feet => valueInFeet,
                LengthUnit.Inch => valueInFeet * 12.0,
                LengthUnit.Yard => valueInFeet / 3.0,
                LengthUnit.Cm => valueInFeet / 0.0328084,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}