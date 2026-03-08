using System;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            QuantityMeasurementService service = new QuantityMeasurementService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Quantity Measurement Menu (UC1 - UC13) =====");
                Console.WriteLine("1. Compare Lengths (Feet/Inch/Yard/Cm)");
                Console.WriteLine("2. Convert Length Units");
                Console.WriteLine("3. Add Lengths");
                Console.WriteLine("4. Add Lengths with Target Unit");
                Console.WriteLine("5. Subtract Lengths");
                Console.WriteLine("6. Subtract Lengths with Target Unit");
                Console.WriteLine("7. Divide Lengths");
                Console.WriteLine("8. Compare Weight");
                Console.WriteLine("9. Convert Weight Units");
                Console.WriteLine("10. Add Weight");
                Console.WriteLine("11. Subtract Weight");
                Console.WriteLine("12. Divide Weight");
                Console.WriteLine("13. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine()!;

                try
                {
                    switch (choice)
                    {
                        // 1: Compare Lengths
                        case "1":
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var q1 = new Quantity<LengthUnit>(v1, u1);
                            var q2 = new Quantity<LengthUnit>(v2, u2);

                            Console.WriteLine("Equal: " + service.GenericAreEqual(q1, q2));
                            break;

                        // 2: Convert Length Units
                        case "2":
                            Console.Write("Enter value: ");
                            double val = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit src = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit tgt = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var conv = service.GenericConvert(new Quantity<LengthUnit>(val, src), tgt);
                            Console.WriteLine($"Converted: {conv.Value} {conv.Unit}");
                            break;

                        // 3: Add Lengths
                        case "3":
                            Console.Write("Enter first value: ");
                            double a1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit au1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double a2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit au2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var addRes = service.GenericAdd(
                                new Quantity<LengthUnit>(a1, au1),
                                new Quantity<LengthUnit>(a2, au2)
                            );
                            Console.WriteLine($"Sum = {addRes.Value} {addRes.Unit}");
                            break;

                        // 4: Add Lengths with Target Unit
                        case "4":
                            Console.Write("Enter first value: ");
                            double b1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit bu1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double b2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit bu2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var addTarget = service.GenericAdd(
                                new Quantity<LengthUnit>(b1, bu1),
                                new Quantity<LengthUnit>(b2, bu2),
                                target
                            );
                            Console.WriteLine($"Sum = {addTarget.Value} {addTarget.Unit}");
                            break;

                        // 5: Subtract Lengths
                        case "5":
                            Console.Write("Enter first value: ");
                            double s1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit su1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double s2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit su2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var subRes = service.GenericSubtract(
                                new Quantity<LengthUnit>(s1, su1),
                                new Quantity<LengthUnit>(s2, su2)
                            );
                            Console.WriteLine($"Subtraction Result = {subRes.Value} {subRes.Unit}");
                            break;

                        // 6: Subtract Lengths with Target Unit
                        case "6":
                            Console.Write("Enter first value: ");
                            double s3 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit su3 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double s4 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit su4 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit subTarget = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var subResTarget = service.GenericSubtract(
                                new Quantity<LengthUnit>(s3, su3),
                                new Quantity<LengthUnit>(s4, su4),
                                subTarget
                            );
                            Console.WriteLine($"Subtraction Result = {subResTarget.Value} {subResTarget.Unit}");
                            break;

                        // 7: Divide Lengths
                        case "7":
                            Console.Write("Enter first value: ");
                            double d1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit du1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double d2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit du2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var divRes = service.GenericDivide(
                                new Quantity<LengthUnit>(d1, du1),
                                new Quantity<LengthUnit>(d2, du2)
                            );
                            Console.WriteLine($"Division Result = {divRes}");
                            break;

                        // 8-12: Weight operations (same pattern)
                        case "8":
                        case "9":
                        case "10":
                        case "11":
                        case "12":
                            Console.WriteLine("Weight operations can be added using same generic methods with WeightUnit.");
                            break;

                        case "13":
                            exit = true;
                            Console.WriteLine("Exiting program...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}