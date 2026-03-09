using System;
using System.Collections.Generic;

namespace QuantityMeasurementApp.Model
{
    public enum TemperatureUnit
    {
        Celsius = 1,
        CELSIUS = Celsius,
        Fahrenheit = 2,
        FAHRENHEIT = Fahrenheit,
        Kelvin = 3,
        KELVIN = Kelvin
    }

    public static class TemperatureUnitExtensions
    {
        // Base unit is Celsius.
        private static readonly Dictionary<TemperatureUnit, Func<double, double>> ToCelsius = new()
        {
            [TemperatureUnit.Celsius] = celsius => celsius,
            [TemperatureUnit.Fahrenheit] = fahrenheit => (fahrenheit - 32.0) * 5.0 / 9.0,
            [TemperatureUnit.Kelvin] = kelvin => kelvin - 273.15
        };

        private static readonly Dictionary<TemperatureUnit, Func<double, double>> FromCelsius = new()
        {
            [TemperatureUnit.Celsius] = celsius => celsius,
            [TemperatureUnit.Fahrenheit] = celsius => (celsius * 9.0 / 5.0) + 32.0,
            [TemperatureUnit.Kelvin] = celsius => celsius + 273.15
        };

        private static readonly SupportsArithmetic SupportsArithmetic = () => false;

        public static string GetUnitName(this TemperatureUnit unit)
        {
            return unit.ToString().ToUpperInvariant();
        }

        public static double GetConversionFactor(this TemperatureUnit unit)
        {
            return 1.0;
        }

        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            if (!ToCelsius.TryGetValue(unit, out Func<double, double>? converter))
                throw new ArgumentException("Invalid TemperatureUnit");

            return converter(value);
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            if (!FromCelsius.TryGetValue(unit, out Func<double, double>? converter))
                throw new ArgumentException("Invalid TemperatureUnit");

            return converter(baseValue);
        }

        public static bool SupportsArithmeticOperation(this TemperatureUnit unit)
        {
            return SupportsArithmetic();
        }

        public static void ValidateOperationSupport(this TemperatureUnit unit, ArithmeticOperation operation)
        {
            throw new NotSupportedException(
                $"Temperature does not support {operation.ToString().ToLowerInvariant()} operation.");
        }
    }
}
