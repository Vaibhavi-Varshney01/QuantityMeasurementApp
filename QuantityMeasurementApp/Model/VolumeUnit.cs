using System;

namespace QuantityMeasurementApp.Model
{
    public enum VolumeUnit
    {
        Litre = 1,
        LITRE = Litre,
        Millilitre = 2,
        MILLILITRE = Millilitre,
        Gallon = 3,
        GALLON = Gallon
    }

    public static class VolumeUnitExtensions
    {
        private const double GALLON_TO_LITRE = 3.78541;
        private const double MILLILITRE_TO_LITRE = 0.001;
        private static readonly SupportsArithmetic SupportsArithmetic = () => true;

        public static double GetConversionFactor(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.Litre => 1.0,
                VolumeUnit.Millilitre => MILLILITRE_TO_LITRE,
                VolumeUnit.Gallon => GALLON_TO_LITRE,
                _ => throw new ArgumentException("Unsupported volume unit")
            };
        }

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.Litre => "Litre",
                VolumeUnit.Millilitre => "Millilitre",
                VolumeUnit.Gallon => "Gallon",
                _ => throw new ArgumentException("Unsupported volume unit")
            };
        }

        public static bool SupportsArithmeticOperation(this VolumeUnit unit)
        {
            return SupportsArithmetic();
        }

        public static void ValidateOperationSupport(this VolumeUnit unit, ArithmeticOperation operation)
        {
            // Volume supports all arithmetic operations.
        }
    }
}
