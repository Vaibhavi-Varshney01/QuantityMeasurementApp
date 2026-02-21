using System;
namespace feet_measurement_equality{
    class Program{
        static void Main(){
            while (true)
            {
                Console.WriteLine("\n===== FEET MEASUREMENT MENU =====");
                Console.WriteLine("1. Compare Two Feet Values");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CompareFeet();
                        break;

                    case "2":
                        Console.WriteLine("Exiting program...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void CompareFeet()
        {
            try
            {
                Console.Write("Enter first value: ");
                string? input1 = Console.ReadLine();

                if (!double.TryParse(input1, out double firstValue))
                {
                    Console.WriteLine("Invalid input!");
                    return;
                }

                Console.Write("Enter second value: ");
                string? input2 = Console.ReadLine();

                if (!double.TryParse(input2, out double secondValue))
                {
                    Console.WriteLine("Invalid input!");
                    return;
                }

                Feet first = new Feet(firstValue);
                Feet second = new Feet(secondValue);

                if (first.Equals(second))
                    Console.WriteLine("Both measurements are equal.");
                else
                    Console.WriteLine("Measurements are NOT equal.");
            }
            catch (InvalidFeetException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}