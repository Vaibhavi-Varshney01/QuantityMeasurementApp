using System;

namespace QuantityMeasurementApp.Model
{
    public enum LengthUnit
    {
        Inch = 1,
        INCHES = Inch,
        Feet = 2,
        FEET = Feet,
        Yard = 3,
        YARD = Yard,
        Cm = 4,
        CENTIMETERS = Cm
    }

    public static class LengthUnitExtensions
    {
        private static readonly SupportsArithmetic SupportsArithmetic = () => true;

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12.0,
                LengthUnit.Yard => value * 3.0,
                LengthUnit.Cm => value / 30.48,
                _ => throw new ArgumentException("Invalid LengthUnit")
            };
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return unit switch
            {
                LengthUnit.Feet => baseValue,
                LengthUnit.Inch => baseValue * 12.0,
                LengthUnit.Yard => baseValue / 3.0,
                LengthUnit.Cm => baseValue * 30.48,
                _ => throw new ArgumentException("Invalid LengthUnit")
            };
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString().ToUpperInvariant();
        }

        public static bool SupportsArithmeticOperation(this LengthUnit unit)
        {
            return SupportsArithmetic();
        }

        public static void ValidateOperationSupport(this LengthUnit unit, ArithmeticOperation operation)
        {
            // Length supports all arithmetic operations.
        }
    }
}
