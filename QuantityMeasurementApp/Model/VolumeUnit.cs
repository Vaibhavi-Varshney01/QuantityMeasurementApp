using System;

namespace QuantityMeasurementApp.Model
{
    public enum VolumeUnit : IMeasurable
    {
        LITRE,
        MILLILITRE,
        GALLON
    }

    public static class VolumeUnitExtensions
    {
        private const double GALLON_TO_LITRE = 3.78541;
        private const double MILLILITRE_TO_LITRE = 0.001;

        public static double GetConversionFactor(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.LITRE => 1.0,
                VolumeUnit.MILLILITRE => MILLILITRE_TO_LITRE,
                VolumeUnit.GALLON => GALLON_TO_LITRE,
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
                VolumeUnit.LITRE => "Litre",
                VolumeUnit.MILLILITRE => "Millilitre",
                VolumeUnit.GALLON => "Gallon",
                _ => throw new ArgumentException("Unsupported volume unit")
            };
        }
    }
}