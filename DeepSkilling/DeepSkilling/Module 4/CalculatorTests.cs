using System;
using NUnit.Framework;
using CalcLibrary;

namespace CalcLibrary.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        private SimpleCalculator _calculator;

        [SetUp]
        public void Init()
        {
            _calculator = new SimpleCalculator();
        }

        [TearDown]
        public void CleanUp()
        {
            _calculator.AllClear();
            _calculator = null;
        }

        [Test]
        [TestCase(10, 5, 15)]
        [TestCase(-3, -7, -10)]
        [TestCase(0, 0, 0)]
        public void Test_Addition(double a, double b, double expectedResult)
        {
            double actualResult = _calculator.Addition(a, b);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(10, 5, 5)]
        [TestCase(-3, -7, 4)]
        [TestCase(0, 0, 0)]
        public void Test_Subtraction(double a, double b, double expectedResult)
        {
            double actualResult = _calculator.Subtraction(a, b);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(10, 5, 50)]
        [TestCase(-3, -7, 21)]
        [TestCase(0, 5, 0)]
        public void Test_Multiplication(double a, double b, double expectedResult)
        {
            double actualResult = _calculator.Multiplication(a, b);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(10, 5, 2)]
        [TestCase(-21, -7, 3)]
        [TestCase(0, 5, 0)]
        public void Test_Division(double a, double b, double expectedResult)
        {
            double actualResult = _calculator.Division(a, b);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_DivisionByZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _calculator.Division(10, 0), "Second Parameter Can't be Zero");
        }

        [Test]
        public void Test_GetResult_InitiallyZero()
        {
            Assert.That(_calculator.GetResult, Is.EqualTo(0));
        }

        [Test]
        public void Test_AllClear_ResetsResultToZero()
        {
            _calculator.Addition(10, 5);
            Assert.That(_calculator.GetResult, Is.EqualTo(15));
            _calculator.AllClear();
            Assert.That(_calculator.GetResult, Is.EqualTo(0));
        }
    }
}
