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
                Console.WriteLine("\n===== Quantity Measurement Menu (UC1 - UC12) =====");
                Console.WriteLine("1. Compare Feet (UC1)");
                Console.WriteLine("2. Compare Inches (UC2)");
                Console.WriteLine("3. Compare Lengths (UC3 + UC4)");
                Console.WriteLine("4. Convert Length Units (UC5)");
                Console.WriteLine("5. Add Lengths (UC6)");
                Console.WriteLine("6. Add Lengths with Target Unit (UC7)");
                Console.WriteLine("7. LengthUnit Conversion Demo (UC8)");
                Console.WriteLine("8. Compare Weight (UC9)");
                Console.WriteLine("9. Convert Weight (UC9)");
                Console.WriteLine("10. Add Weight (UC9)");
                Console.WriteLine("11. Generic Equality (UC10)");
                Console.WriteLine("12. Generic Conversion (UC10)");
                Console.WriteLine("13. Generic Addition (UC10)");
                Console.WriteLine("14. Subtract Quantity (UC12)");
                Console.WriteLine("15. Subtract Quantity with Target Unit (UC12)");
                Console.WriteLine("16. Divide Quantities (UC12)");
                Console.WriteLine("17. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine()!;

                try
                {
                    switch (choice)
                    {
                        // UC1
                        case "1":
                            Console.Write("Enter first feet: ");
                            double f1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second feet: ");
                            double f2 = double.Parse(Console.ReadLine()!);
                            Console.WriteLine("Equal: " + service.AreEqual(new Quantity<LengthUnit>(f1, LengthUnit.Feet),
                                                                         new Quantity<LengthUnit>(f2, LengthUnit.Feet)));
                            break;

                        // UC2
                        case "2":
                            Console.Write("Enter first inches: ");
                            double i1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second inches: ");
                            double i2 = double.Parse(Console.ReadLine()!);
                            Console.WriteLine("Equal: " + service.AreEqual(new Quantity<LengthUnit>(i1, LengthUnit.Inch),
                                                                         new Quantity<LengthUnit>(i2, LengthUnit.Inch)));
                            break;

                        // UC3 + UC4
                        case "3":
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

                        // UC5
                        case "4":
                            Console.Write("Enter value: ");
                            double val = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit src = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit tgt = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var converted = service.Convert(new Quantity<LengthUnit>(val, src), tgt);
                            Console.WriteLine($"Converted: {converted.Value} {converted.Unit}");
                            break;

                        // UC6
                        case "5":
                            Console.Write("Enter first value: ");
                            double a1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit au1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double a2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit au2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var sum = service.Add(new Quantity<LengthUnit>(a1, au1),
                                                  new Quantity<LengthUnit>(a2, au2));
                            Console.WriteLine($"Sum = {sum.Value} {sum.Unit}");
                            break;

                        // UC7
                        case "6":
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
                            var sumTarget = service.Add(new Quantity<LengthUnit>(b1, bu1),
                                                        new Quantity<LengthUnit>(b2, bu2),
                                                        target);
                            Console.WriteLine($"Sum = {sumTarget.Value} {sumTarget.Unit}");
                            break;

                        // UC8 Demo
                        case "7":
                            var demoFoot = new Quantity<LengthUnit>(1, LengthUnit.Feet);
                            var demoInch = service.Convert(demoFoot, LengthUnit.Inch);
                            var demoCm = service.Convert(demoFoot, LengthUnit.Cm);
                            Console.WriteLine($"1 Foot = {demoInch.Value} Inch = {demoCm.Value} Cm");
                            break;

                        // UC9 - Weight
                        case "8":
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

                        case "9":
                            Console.Write("Enter value: ");
                            double wVal = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Kg/Gm/Lb): ");
                            WeightUnit ws = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Kg/Gm/Lb): ");
                            WeightUnit wt = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            var wConverted = service.Convert(new Quantity<WeightUnit>(wVal, ws), wt);
                            Console.WriteLine($"Converted: {wConverted.Value} {wConverted.Unit}");
                            break;

                        case "10":
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

                        // UC10 - Generic
                        case "11":
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

                        case "12":
                            Console.Write("Enter value: ");
                            double convVal = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit convUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit convTarget = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var convQ = new Quantity<LengthUnit>(convVal, convUnit);
                            var convertedGeneric = service.GenericConvert(convQ, convTarget);
                            Console.WriteLine($"Converted = {convertedGeneric.Value} {convertedGeneric.Unit}");
                            break;

                        case "13":
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
                            var addResult = service.GenericAdd(addQ1, addQ2, addTarget);
                            Console.WriteLine($"Addition Result = {addResult.Value} {addResult.Unit}");
                            break;

                        // UC12 - Subtraction
                        case "14":
                            Console.Write("Enter first value: ");
                            double s1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit su1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double s2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit su2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var subQ1 = new Quantity<LengthUnit>(s1, su1);
                            var subQ2 = new Quantity<LengthUnit>(s2, su2);
                            var subResult = service.GenericSubtract(subQ1, subQ2); // UC12 implicit
                            Console.WriteLine($"Subtraction Result = {subResult.Value} {subResult.Unit}");
                            break;

                        case "15":
                            Console.Write("Enter first value: ");
                            double s3 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit su3 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double s4 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit su4 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit targetUnitSub = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var subResultTarget = service.GenericSubtract(subQ1, subQ2, targetUnitSub); // UC12 explicit
                            Console.WriteLine($"Subtraction Result = {subResultTarget.Value} {subResultTarget.Unit}");
                            break;

                        case "16":
                            Console.Write("Enter first value: ");
                            double d1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit du1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double d2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit du2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            var divQ1 = new Quantity<LengthUnit>(d1, du1);
                            var divQ2 = new Quantity<LengthUnit>(d2, du2);
                            var divResult = service.GenericDivide(divQ1, divQ2);
                            Console.WriteLine($"Division Result = {divResult}");
                            break;

                        case "17":
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