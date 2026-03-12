namespace QuantityMeasurementModel
{
    public interface IMeasurable
    {
        string GetUnitName();
        double ConvertToBase(double value);
    }
}