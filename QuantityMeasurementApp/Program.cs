using System;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main()
        {
            QuantityMeasurementService service = new QuantityMeasurementService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Quantity Measurement App ---");
                Console.WriteLine("1. Check Feet Equality");
                Console.WriteLine("2. Exit");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input! Enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        // Get first value
                        Console.Write("Enter first value in feet: ");
                        if (!double.TryParse(Console.ReadLine(), out double firstVal))
                        {
                            Console.WriteLine("Invalid number! Try again.");
                            break;
                        }

                        // Get second value
                        Console.Write("Enter second value in feet: ");
                        if (!double.TryParse(Console.ReadLine(), out double secondVal))
                        {
                            Console.WriteLine("Invalid number! Try again.");
                            break;
                        }

                        // Compare using service
                        Feet first = new Feet(firstVal);
                        Feet second = new Feet(secondVal);

                        bool result = service.AreFeetEqual(first, second);
                        Console.WriteLine(result ? "Equal (true)" : "Not Equal (false)");
                        break;

                    case 2:
                        exit = true;
                        Console.WriteLine("Exiting app... Bye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Enter 1 or 2.");
                        break;
                }
            }
        }
    }
}