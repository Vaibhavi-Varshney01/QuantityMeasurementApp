namespace QuantityMeasurementApp.Unit
{
    public interface IMeasurable
    {
        double GetConversionFactor();              // Factor to base unit
        double ConvertToBaseUnit(double value);   // Convert value to base unit
        double ConvertFromBaseUnit(double baseValue); // Convert from base
        string GetUnitName();                      // Human readable name
    }
}