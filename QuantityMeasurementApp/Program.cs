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
                Console.WriteLine("\n===== Quantity Measurement Menu =====");
                Console.WriteLine("1. Compare Feet (UC1)");
                Console.WriteLine("2. Compare Inches (UC2)");
                Console.WriteLine("3. Compare Lengths (UC3 + UC4)");
                Console.WriteLine("4. Convert Units (UC5)");
                Console.WriteLine("5. Add Lengths (UC6)");
                Console.WriteLine("6. Add Lengths with Target Unit (UC7)");
                Console.WriteLine("7. LengthUnit Conversion Demo (UC8)");
                Console.WriteLine("8. Compare Weight (UC9)");
                Console.WriteLine("9. Convert Weight (UC9)");
                Console.WriteLine("10. Add Weight (UC9)");
                Console.WriteLine("11. Generic Operations Demo (UC10 + UC11)");
                Console.WriteLine("12. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            // UC1 - Compare Feet
                            Console.Write("Enter first feet value: ");
                            double f1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second feet value: ");
                            double f2 = double.Parse(Console.ReadLine()!);
                            Console.WriteLine("Equal: " +
                                service.AreEqual(
                                    new Quantity<LengthUnit>(f1, LengthUnit.Feet),
                                    new Quantity<LengthUnit>(f2, LengthUnit.Feet)));
                            break;

                        case "2":
                            // UC2 - Compare Inches
                            Console.Write("Enter first inch value: ");
                            double i1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second inch value: ");
                            double i2 = double.Parse(Console.ReadLine()!);
                            Console.WriteLine("Equal: " +
                                service.AreEqual(
                                    new Quantity<LengthUnit>(i1, LengthUnit.Inch),
                                    new Quantity<LengthUnit>(i2, LengthUnit.Inch)));
                            break;

                        case "3":
                            // UC3 + UC4 - Compare any LengthUnit
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.WriteLine("Equal: " + service.AreEqual(
                                new Quantity<LengthUnit>(v1, u1),
                                new Quantity<LengthUnit>(v2, u2)));
                            break;

                        case "4":
                            // UC5 - Convert Length
                            Console.Write("Enter value: ");
                            double val = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit src = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit tgt = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var converted = service.Convert(new Quantity<LengthUnit>(val, src), tgt);
                            Console.WriteLine($"Converted = {converted.Value} {converted.Unit}");
                            break;

                        case "5":
                            // UC6 - Add Lengths
                            Console.Write("Enter first value: ");
                            double a1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit au1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double a2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit au2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var sum = service.Add(
                                new Quantity<LengthUnit>(a1, au1),
                                new Quantity<LengthUnit>(a2, au2));
                            Console.WriteLine($"Sum = {sum.Value} {sum.Unit}");
                            break;

                        case "6":
                            // UC7 - Add Lengths with Target Unit
                            Console.Write("Enter first value: ");
                            double b1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit bu1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double b2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit bu2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit tgtUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var result = service.Add(
                                new Quantity<LengthUnit>(b1, bu1),
                                new Quantity<LengthUnit>(b2, bu2),
                                tgtUnit);
                            Console.WriteLine($"Sum = {result.Value} {result.Unit}");
                            break;

                        case "7":
                            // UC8 - Length Demo
                            Console.WriteLine("Converting 1 Foot to Inches and Cm");
                            var footQty = new Quantity<LengthUnit>(1, LengthUnit.Feet);
                            var inchQty = service.Convert(footQty, LengthUnit.Inch);
                            var cmQty = service.Convert(footQty, LengthUnit.Cm);
                            Console.WriteLine($"1 Foot = {inchQty.Value} Inch = {cmQty.Value} Cm");
                            break;

                        case "8":
                            // UC9 - Compare Weight
                            Console.Write("Enter first weight: ");
                            double w1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Kg/Gm/Lb): ");
                            WeightUnit wu1 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second weight: ");
                            double w2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Kg/Gm/Lb): ");
                            WeightUnit wu2 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.WriteLine("Equal: " +
                                service.AreEqual(
                                    new Quantity<WeightUnit>(w1, wu1),
                                    new Quantity<WeightUnit>(w2, wu2)));
                            break;

                        case "9":
                            // UC9 - Convert Weight
                            Console.Write("Enter value: ");
                            double wVal = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Kg/Gm/Lb): ");
                            WeightUnit ws = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Kg/Gm/Lb): ");
                            WeightUnit wt = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            var wConverted = service.Convert(new Quantity<WeightUnit>(wVal, ws), wt);
                            Console.WriteLine($"Converted = {wConverted.Value} {wConverted.Unit}");
                            break;

                        case "10":
                            // UC9 - Add Weight
                            Console.Write("Enter first value: ");
                            double wA1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Kg/Gm/Lb): ");
                            WeightUnit wU1 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double wA2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Kg/Gm/Lb): ");
                            WeightUnit wU2 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Kg/Gm/Lb): ");
                            WeightUnit wTarget = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            var wSum = service.Add(
                                new Quantity<WeightUnit>(wA1, wU1),
                                new Quantity<WeightUnit>(wA2, wU2),
                                wTarget);
                            Console.WriteLine($"Sum = {wSum.Value} {wSum.Unit}");
                            break;

                        case "11":
                            // UC10 + UC11 - Generic Operations Demo
                            Console.Write("Enter first value: ");
                            double g1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit gu1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double g2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit gu2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var q1Gen = new Quantity<LengthUnit>(g1, gu1);
                            var q2Gen = new Quantity<LengthUnit>(g2, gu2);
                            Console.WriteLine("---- UC11 Extended Generic Demo ----");
                            service.UC11_Demo(q1Gen, q2Gen, target);
                            break;

                        case "12":
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