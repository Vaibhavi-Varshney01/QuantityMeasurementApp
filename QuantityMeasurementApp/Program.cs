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
                Console.WriteLine("11. Generic Equality (UC10)");
                Console.WriteLine("12. Generic Conversion (UC10)");
                Console.WriteLine("13. Generic Addition (UC10)");
                Console.WriteLine("14. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        // UC1 - Compare Feet
                        case "1":
                            Console.Write("Enter first feet value: ");
                            double f1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second feet value: ");
                            double f2 = double.Parse(Console.ReadLine()!);
                            Console.WriteLine("Equal: " + service.AreEqual(new Quantity<LengthUnit>(f1, LengthUnit.Feet),
                                                                         new Quantity<LengthUnit>(f2, LengthUnit.Feet)));
                            break;

                        // UC2 - Compare Inches
                        case "2":
                            Console.Write("Enter first inches value: ");
                            double i1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second inches value: ");
                            double i2 = double.Parse(Console.ReadLine()!);
                            Console.WriteLine("Equal: " + service.AreEqual(new Quantity<LengthUnit>(i1, LengthUnit.Inch),
                                                                         new Quantity<LengthUnit>(i2, LengthUnit.Inch)));
                            break;

                        // UC3 + UC4 - Compare any LengthUnit
                        case "3":
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
                            Console.WriteLine("Equal: " + service.AreEqual(q1, q2));
                            break;

                        // UC5 - Convert LengthUnit
                        case "4":
                            Console.Write("Enter value: ");
                            double val = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit src = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit tgt = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var converted = service.Convert(new Quantity<LengthUnit>(val, src), tgt);
                            Console.WriteLine($"Converted = {converted.Value} {converted.Unit}");
                            break;

                        // UC6 - Add Lengths
                        case "5":
                            Console.Write("Enter first value: ");
                            double a1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit au1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double a2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit au2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var sum = service.Add(new Quantity<LengthUnit>(a1, au1),
                                                  new Quantity<LengthUnit>(a2, au2));
                            Console.WriteLine($"Sum = {sum.Value} {sum.Unit}");
                            break;

                        // UC7 - Add Lengths with Target Unit
                        case "6":
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

                            var result = service.Add(new Quantity<LengthUnit>(b1, bu1),
                                                     new Quantity<LengthUnit>(b2, bu2),
                                                     tgtUnit);
                            Console.WriteLine($"Sum = {result.Value} {result.Unit}");
                            break;

                        // UC8 - LengthUnit Conversion Demo
                        case "7":
                            Console.WriteLine("UC8 Demo: Converting 1 Foot to Inches and Cm");
                            var footQty = new Quantity<LengthUnit>(1, LengthUnit.Feet);
                            var inchQty = service.Convert(footQty, LengthUnit.Inch);
                            var cmQty = service.Convert(footQty, LengthUnit.Cm);
                            Console.WriteLine($"1 Foot = {inchQty.Value} Inch = {cmQty.Value} Cm");
                            break;

                        // UC9 - Weight Operations
                        case "8": // Compare Weight
                            Console.Write("Enter first weight: ");
                            double w1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Kg/Gm/Lb): ");
                            WeightUnit wu1 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second weight: ");
                            double w2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Kg/Gm/Lb): ");
                            WeightUnit wu2 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);

                            Console.WriteLine("Equal: " + service.AreEqual(new Quantity<WeightUnit>(w1, wu1),
                                                                          new Quantity<WeightUnit>(w2, wu2)));
                            break;

                        case "9": // Convert Weight
                            Console.Write("Enter value: ");
                            double wVal = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Kg/Gm/Lb): ");
                            WeightUnit ws = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Kg/Gm/Lb): ");
                            WeightUnit wt = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);

                            var wConverted = service.Convert(new Quantity<WeightUnit>(wVal, ws), wt);
                            Console.WriteLine($"Converted = {wConverted.Value} {wConverted.Unit}");
                            break;

                        case "10": // Add Weight
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

                            var wSum = service.Add(new Quantity<WeightUnit>(wA1, wU1),
                                                   new Quantity<WeightUnit>(wA2, wU2),
                                                   wTarget);
                            Console.WriteLine($"Sum = {wSum.Value} {wSum.Unit}");
                            break;

                        // UC10 - Generic Operations
                        case "11": // Generic Equality
                            Console.Write("Enter first value: ");
                            double g1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit gu1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double g2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit gu2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var genQ1 = new Quantity<LengthUnit>(g1, gu1);
                            var genQ2 = new Quantity<LengthUnit>(g2, gu2);
                            Console.WriteLine("Generic Equality Result: " + service.GenericAreEqual(genQ1, genQ2));
                            break;

                        case "12": // Generic Conversion
                            Console.Write("Enter value: ");
                            double convVal = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit convUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit convTarget = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var convQ = new Quantity<LengthUnit>(convVal, convUnit);
                            var convertedGenericQ = service.GenericConvert(convQ, convTarget);
                            Console.WriteLine($"Converted = {convertedGenericQ.Value} {convertedGenericQ.Unit}");
                            break;

                        case "13": // Generic Addition
                            Console.Write("Enter first value: ");
                            double addVal1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit addUnit1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double addVal2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit addUnit2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit addTarget = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var addQ1 = new Quantity<LengthUnit>(addVal1, addUnit1);
                            var addQ2 = new Quantity<LengthUnit>(addVal2, addUnit2);
                            var addGenericResult = service.GenericAdd(addQ1, addQ2, addTarget);
                            Console.WriteLine($"Addition Result = {addGenericResult.Value} {addGenericResult.Unit}");
                            break;

                        case "14":
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