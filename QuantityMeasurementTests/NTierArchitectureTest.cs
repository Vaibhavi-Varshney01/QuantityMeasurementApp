using NUnit.Framework;
using QuantityMeasurementModel;
using QuantityMeasurementModel.Models;
using QuantityMeasurementRepository;
using QuantityMeasurementControllerLayer;
using QuantityMeasurementModel.Entities;
using System;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementAppTest
    {
        private QuantityMeasurementService service;
        private QuantityMeasurementController controller;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
            controller = new QuantityMeasurementController(service);
        }

        // ================================
        // ENTITY LAYER TESTS
        // ================================

        [Test]
        public void testQuantityEntity_SingleOperandConstruction()
        {
            var entity = new QuantityMeasurementEntity(
                new QuantityModel<LengthUnit> { Value = 1, Unit = LengthUnit.Feet },
                "CONVERT",
                new QuantityModel<LengthUnit> { Value = 12, Unit = LengthUnit.Inch }
            );

            Assert.IsNotNull(entity);
            Assert.AreEqual("CONVERT", entity.OperationType);
        }

        [Test]
        public void testQuantityEntity_BinaryOperandConstruction()
        {
            var entity = new QuantityMeasurementEntity(
                new QuantityModel<LengthUnit> { Value = 1, Unit = LengthUnit.Feet },
                new QuantityModel<LengthUnit> { Value = 12, Unit = LengthUnit.Inch },
                "ADD",
                new QuantityModel<LengthUnit> { Value = 2, Unit = LengthUnit.Feet }
            );

            Assert.IsNotNull(entity);
            Assert.AreEqual("ADD", entity.OperationType);
        }

        [Test]
        public void testQuantityEntity_ErrorConstruction()
        {
            var entity = new QuantityMeasurementEntity("Division by zero");

            Assert.IsTrue(entity.HasError);
            Assert.AreEqual("Division by zero", entity.ErrorMessage);
        }

        // ================================
        // SERVICE LAYER TESTS
        // ================================

        [Test]
        public void testService_CompareEquality_SameUnit_Success()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            Assert.IsTrue(service.AreEqual(q1, q2));
        }

        [Test]
        public void testService_CompareEquality_DifferentUnit_Success()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            Assert.IsTrue(service.AreEqual(q1, q2));
        }

        [Test]
        public void testService_CompareEquality_CrossCategory_Error()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<WeightUnit>(1, WeightUnit.Kg);

            Assert.Throws<ArgumentException>(() =>
            {
                service.GenericAreEqual(q1, (dynamic)q2);
            });
        }

        [Test]
        public void testService_Convert_Success()
        {
            var quantity = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = service.GenericConvert(quantity, LengthUnit.Inch);

            Assert.AreEqual(12, result.Value);
        }

        [Test]
        public void testService_Add_Success()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.AreEqual(2, result.Value);
        }

        [Test]
        public void testService_Add_UnsupportedOperation_Error()
        {
            var q1 = new Quantity<TemperatureUnit>(10, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(20, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() =>
            {
                service.Add(q1, q2);
            });
        }

        [Test]
        public void testService_Subtract_Success()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = service.Subtract(q1, q2, LengthUnit.Feet);

            Assert.AreEqual(4, result.Value);
        }

        [Test]
        public void testService_Divide_Success()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = service.Divide(q1, q2);

            Assert.AreEqual(2, result);
        }

        [Test]
        public void testService_Divide_ByZero_Error()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0, LengthUnit.Feet);

            Assert.Throws<ArithmeticException>(() =>
            {
                service.Divide(q1, q2);
            });
        }

        // ================================
        // CONTROLLER TESTS
        // ================================

        [Test]
        public void testController_DemonstrateEquality_Success()
        {
            var result = controller.PerformEqualityDemo();

            Assert.IsTrue(result);
        }

        [Test]
        public void testController_DemonstrateConversion_Success()
        {
            var result = controller.PerformConversionDemo();

            Assert.IsNotNull(result);
        }

        [Test]
        public void testController_DemonstrateAddition_Success()
        {
            var result = controller.PerformAdditionDemo();

            Assert.IsNotNull(result);
        }

        // ================================
        // LAYER SEPARATION TESTS
        // ================================

        [Test]
        public void testLayerSeparation_ServiceIndependence()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            Assert.IsTrue(service.AreEqual(q1, q2));
        }

        [Test]
        public void testLayerSeparation_ControllerIndependence()
        {
            Assert.DoesNotThrow(() =>
            {
                controller.PerformAdditionDemo();
            });
        }

        // ================================
        // DATA FLOW TESTS
        // ================================

        [Test]
        public void testDataFlow_ControllerToService()
        {
            var result = controller.PerformAdditionDemo();

            Assert.IsNotNull(result);
        }

        [Test]
        public void testDataFlow_ServiceToController()
        {
            var result = controller.PerformConversionDemo();

            Assert.IsNotNull(result);
        }

        // ================================
        // VALIDATION TESTS
        // ================================

        [Test]
        public void testService_NullEntity_Rejection()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                service.Add(null, null);
            });
        }

        [Test]
        public void testController_NullService_Prevention()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new QuantityMeasurementController(null);
            });
        }

        // ================================
        // INTEGRATION TESTS
        // ================================

        [Test]
        public void testIntegration_EndToEnd_LengthAddition()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.AreEqual(2, result.Value);
        }

        [Test]
        public void testIntegration_EndToEnd_TemperatureUnsupported()
        {
            var q1 = new Quantity<TemperatureUnit>(10, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(20, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() =>
            {
                service.Add(q1, q2);
            });
        }

        // ================================
        // BACKWARD COMPATIBILITY TEST
        // ================================

        [Test]
        public void testBackwardCompatibility_AllUC1_UC14_Tests()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            Assert.IsTrue(service.AreEqual(q1, q2));
        }
    }
}