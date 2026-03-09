namespace QuantityMeasurementApp.Model
{
    public enum ArithmeticOperation
    {
        Add,
        Subtract,
        Divide
    }

    public delegate bool SupportsArithmetic();

    public interface IMeasurable
    {
        string GetUnitName();
        double GetConversionFactor();
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);

        // All measurable units support arithmetic by default unless overridden.
        bool SupportsArithmeticOperation() => true;

        // Units can override this to reject specific operations.
        void ValidateOperationSupport(ArithmeticOperation operation)
        {
            // Default behavior intentionally does nothing.
        }
    }
}
