using System;

namespace yard_equality
{
    class Program
    {
        static LengthUnit GetUnit(string input)
        {
            input = input.ToUpper();

            switch (input)
            {
                case "INCHES":
                    return LengthUnit.INCHES;

                case "FEET":
                    return LengthUnit.FEET;

                case "YARDS":
                    return LengthUnit.YARDS;

                case "CENTIMETERS":
                    return LengthUnit.CENTIMETERS;

                default:
                    throw new InvalidUnitException("Invalid Unit Entered");
            }
        }

        static void Main()
        {
            try
            {
                Console.WriteLine("Enter first value:");
                double value1 = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Enter first unit (INCHES / FEET / YARDS / CENTIMETERS):");
                LengthUnit unit1 = GetUnit(Console.ReadLine());

                Console.WriteLine("Enter second value:");
                double value2 = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Enter second unit (INCHES / FEET / YARDS / CENTIMETERS):");
                LengthUnit unit2 = GetUnit(Console.ReadLine());

                QuantityLength q1 = new QuantityLength(value1, unit1);
                QuantityLength q2 = new QuantityLength(value2, unit2);

                QuantityMeasurementApp.Compare(q1, q2);
            }
            catch (InvalidLengthException e)
            {
                Console.WriteLine("Length Error: " + e.Message);
            }
            catch (InvalidUnitException e)
            {
                Console.WriteLine("Unit Error: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: " + e.Message);
            }
        }
    }
}