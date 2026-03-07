using System;
using feet_and_inches_equality;

class Program
{
    static void Main()
    {
        QuantityMeasurementApp app = new QuantityMeasurementApp();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n=== Quantity Measurement Menu ===");
            Console.WriteLine("1. Compare Feet");
            Console.WriteLine("2. Compare Inches");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1-3): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CompareFeet(app);
                    break;

                case "2":
                    CompareInches(app);
                    break;

                case "3":
                    exit = true;
                    Console.WriteLine("Exiting program. Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    break;
            }
        }
    }

    // Feet comparison method
    static void CompareFeet(QuantityMeasurementApp app)
    {
        Console.Write("Enter first feet value: ");
        double firstFeet = ReadDoubleInput();

        Console.Write("Enter second feet value: ");
        double secondFeet = ReadDoubleInput();

        bool result = app.CompareFeet(firstFeet, secondFeet);
        Console.WriteLine($"Feet comparison: {firstFeet} ft and {secondFeet} ft -> Equal ({result})");
    }

    // Inches comparison method
    static void CompareInches(QuantityMeasurementApp app)
    {
        Console.Write("Enter first inches value: ");
        double firstInches = ReadDoubleInput();

        Console.Write("Enter second inches value: ");
        double secondInches = ReadDoubleInput();

        bool result = app.CompareInches(firstInches, secondInches);
        Console.WriteLine($"Inches comparison: {firstInches} inch and {secondInches} inch -> Equal ({result})");
    }

    // Helper method to safely read double
    static double ReadDoubleInput()
    {
        double value;
        while (true)
        {
            string? input = Console.ReadLine();
            if (double.TryParse(input, out value) && value >= 0)
                return value;

            Console.Write("Invalid input. Enter a positive number: ");
        }
    }
}