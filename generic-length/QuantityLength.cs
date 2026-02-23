using System;

namespace generic_length
{

    //<summary>
    /// Represents a generic length measurement.
    /// Implements the DRY principle by consolidating Feet/Inch logic into one class.
    /// Provides unit conversion, validation, and equality comparison.
    //</summary>
    public class QuantityLength : IEquatable<QuantityLength>
    {
        private double value;
        private LengthUnit unit;

                /// Initializes a new instance of QuantityLength with a value and unit.
        /// Throws InvalidLengthException if value is negative or unit is invalid.

       public QuantityLength(double value, LengthUnit? unit)
{
    if (value < 0)
        throw new InvalidLengthException("Value cannot be negative.");

    if (unit == null || !Enum.IsDefined(typeof(LengthUnit), unit.Value))
        throw new InvalidLengthException("Invalid unit.");

    this.value = value;
    this.unit = unit.Value;
}

 /// Converts the current quantity to inches for comparison purposes.

        private double ToInches()
        {
            if (unit == LengthUnit.FEET)
                return value * 12;

            return value;
        }

         /// Determines whether this instance is equal to another QuantityLength.

        public bool Equals(QuantityLength? other)
        {
            if (other == null)
                return false;

            return this.ToInches() == other.ToInches();
        }

        /// Overrides object.Equals for type-safe equality check

        public override bool Equals(object? obj)
        {
            if (obj is QuantityLength other)
                return Equals(other);

            return false;
        }

        /// Returns a hash code based on the value in inches

        public override int GetHashCode()
        {
            return ToInches().GetHashCode();
        }
    }
}