using System;

public static class LengthUnitExtended
{
    public static double ToFeet(this LengthUnit unit, double value)
    {
        switch (unit)
        {
            case LengthUnit.FEET:
                return value;

            case LengthUnit.INCH:
                return value / 12.0;

            default:
                throw new ArgumentException("Unsupported unit");
        }
    }
}