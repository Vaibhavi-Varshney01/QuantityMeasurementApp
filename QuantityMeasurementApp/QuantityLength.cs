using System;

namespace yard_equality
{
    public class QuantityLength
    {
    public double Value { get; }
    public LengthUnit Unit { get; }

    public QuantityLength(double value, LengthUnit unit)
    {
        if (value < 0)
            throw new InvalidLengthException("Length cannot be negative");

        Value = value;
        Unit = unit;
    }

    private double ConvertToInches()
    {
        switch (Unit)
        {
            case LengthUnit.INCHES:
                return Value;

            case LengthUnit.FEET:
                return Value * 12;

            case LengthUnit.YARDS:
                return Value * 36;

            case LengthUnit.CENTIMETERS:
                return Value * 0.393701;

            default:
                throw new InvalidUnitException("Unsupported Unit");
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is QuantityLength))
            return false;

        QuantityLength other = (QuantityLength)obj;

        double first = this.ConvertToInches();
        double second = other.ConvertToInches();

        return Math.Abs(first - second) < 0.0001;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Unit);
    }

    public override string ToString()
    {
        return $"Quantity({Value}, {Unit})";
    }
}
}