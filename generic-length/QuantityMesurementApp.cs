using System;
using generic_length;

namespace generic_length{

public class QuantityMeasurementApp
{
    private QuantityLength q1 = null!;
    private QuantityLength q2 = null!;

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n======WELCOME TO GENERIC LENGTH UC-3======");
            Console.WriteLine("\n1. Enter Quantities");
            Console.WriteLine("2. Compare");
            Console.WriteLine("3. Exit");
            Console.Write("Choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        q1 = ReadQuantity("First");
                        q2 = ReadQuantity("Second");
                        break;

                    case 2:
                        CompareQuantities();
                        break;

                    case 3:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (InvalidLengthException ex)
            {
                Console.WriteLine("Custom Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
            }
        }
    }

    private QuantityLength ReadQuantity(string label)
    {
        Console.Write($"{label} Value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        Console.Write($"{label} Unit (Feet/Inch): ");
        string input = Console.ReadLine();

        if (!Enum.TryParse(input, true, out LengthUnit unit))
            throw new InvalidLengthException("Invalid unit entered.");

        return new QuantityLength(value, unit);
    }

    private void CompareQuantities()
    {
        if (q1 == null || q2 == null)
        {
            Console.WriteLine("Enter quantities first.");
            return;
        }

        Console.WriteLine("Equal: " + q1.Equals(q2));
    }
}
}