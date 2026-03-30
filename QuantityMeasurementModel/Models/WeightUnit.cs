namespace QuantityMeasurementModel.Models
{
    public enum WeightUnit
    {
        Kilogram = 0,
        Gram = 1,
        Pound = 2,
        Tonne = 3,

        // Synonyms used elsewhere
        Kg = Kilogram,
        Gm = Gram,
        Lb = Pound,
        // Legacy uppercase names
        KILOGRAM = Kilogram,
        GRAM = Gram,
        POUND = Pound,
        TONNE = Tonne
    }

    public static class WeightUnitExtensions
    {
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                // base unit: Gram
                WeightUnit.Gram => 1.0,
                WeightUnit.Kilogram => 1000.0,
                WeightUnit.Pound => 453.592,
                WeightUnit.Tonne => 1_000_000.0,
                _ => throw new ArgumentException("Invalid Unit")
            };
        }

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString().ToUpperInvariant();
        }
    }
}
