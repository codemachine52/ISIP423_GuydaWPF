using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryCalculator;

namespace SalaryCalculatorTest
{
    [TestClass]
    public class SalaryServiceTests
    {
        private SalaryService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new SalaryService();
        }

        [TestMethod]
        public void Test_Assistant_NoTax()
        {
            // Тест-кейс TC_UI_01 [cite: 7]
            // Arrange
            double hours = 10;
            Position pos = Position.Assistant;
            bool includeTax = false;

            // Act
            var result = _service.Calculate(hours, pos, includeTax);

            // Assert
            Assert.AreEqual(1500.00, result.GrossSalary, "Начислено для ассистента за 10ч должно быть 1500");
            Assert.AreEqual(0.00, result.TaxAmount, "Налог должен быть 0, если чекбокс не выбран");
        }

        [TestMethod]
        public void Test_Docent_WithTax()
        {
            // Тест-кейс TC_UI_02 [cite: 9]
            // Arrange
            double hours = 20;
            Position pos = Position.Docent;
            bool includeTax = true;

            // Act
            var result = _service.Calculate(hours, pos, includeTax);

            // Assert
            Assert.AreEqual(5000.00, result.GrossSalary, "Начислено для доцента за 20ч (250 р/ч) должно быть 5000");
            Assert.AreEqual(650.00, result.TaxAmount, "Налог 13% от 5000 должен быть 650");
        }

        [TestMethod]
        public void Test_Professor_FractionalHours()
        {
            // Тест-кейс TC_UI_03 [cite: 11]
            // Arrange
            double hours = 5.5;
            Position pos = Position.Professor;
            bool includeTax = true;

            // Act
            var result = _service.Calculate(hours, pos, includeTax);

            // Assert
            Assert.AreEqual(1925.00, result.GrossSalary, "Начислено для профессора за 5.5ч (350 р/ч) должно быть 1925");
            Assert.AreEqual(250.25, result.TaxAmount, "Налог 13% от 1925 должен быть 250.25");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_NegativeHours_ThrowsException()
        {
            // Тест-кейс TC_VAL_02 
            // Act
            _service.Calculate(-10, Position.Assistant, false);

            // Assert выполняется автоматически атрибутом ExpectedException
        }

        [TestMethod]
        public void Test_ZeroHours()
        {
            // Arrange & Act
            var result = _service.Calculate(0, Position.Assistant, true);

            // Assert
            Assert.AreEqual(0, result.GrossSalary);
            Assert.AreEqual(0, result.TaxAmount);
        }
    }
}