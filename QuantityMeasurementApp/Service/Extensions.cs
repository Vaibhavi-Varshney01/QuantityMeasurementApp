using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public static class Extensions
    {
        public static double ConvertTo(this double value, LengthUnit from, LengthUnit to)
        {
            double inches = from switch
            {
                LengthUnit.Inch => value,
                LengthUnit.Feet => value * 12,
                LengthUnit.Yard => value * 36,
                _ => throw new Exception("Invalid Length Unit")
            };

            return to switch
            {
                LengthUnit.Inch => inches,
                LengthUnit.Feet => inches / 12,
                LengthUnit.Yard => inches / 36,
                _ => throw new Exception("Invalid Length Unit")
            };
        }

        public static double ConvertTo(this double value, WeightUnit from, WeightUnit to)
        {
            double grams = from switch
            {
                WeightUnit.Gram => value,
                WeightUnit.Kilogram => value * 1000,
                WeightUnit.Tonne => value * 1_000_000,
                _ => throw new Exception("Invalid Weight Unit")
            };

            return to switch
            {
                WeightUnit.Gram => grams,
                WeightUnit.Kilogram => grams / 1000,
                WeightUnit.Tonne => grams / 1_000_000,
                _ => throw new Exception("Invalid Weight Unit")
            };
        }

        public static double Add(this double v1, LengthUnit unit1, double v2, LengthUnit unit2, LengthUnit resultUnit)
        {
            double convertedV1 = v1.ConvertTo(unit1, resultUnit);
            double convertedV2 = v2.ConvertTo(unit2, resultUnit);
            return convertedV1 + convertedV2;
        }

        public static double Add(this double v1, WeightUnit unit1, double v2, WeightUnit unit2, WeightUnit resultUnit)
        {
            double convertedV1 = v1.ConvertTo(unit1, resultUnit);
            double convertedV2 = v2.ConvertTo(unit2, resultUnit);
            return convertedV1 + convertedV2;
        }
    }
}