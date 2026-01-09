using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcStressTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "stress", "σ");

            // Act
            // Assert
            Assert.True(CalcStress.TryParse("5.5 kPa", null, out calcStress));
            Assert.Equal(5.5, calcStress.Value);
            Assert.Equal("kPa", calcStress.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcStress = CalcStress.Parse("5.5 kPa", null);

            // Assert
            Assert.Equal(5.5, calcStress.Value);
            Assert.Equal("kPa", calcStress.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "stress", "σ");

            // Act
            // Assert
            Assert.False(CalcStress.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcStress = new Pressure(4.5, PressureUnit.Gigapascal);

            // Act
            CalcStress value = calcStress;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.KilonewtonPerSquareMeter, "stress", "σ");

            // Act
            double value = calcStress;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.KilonewtonPerSquareMeter, "stress", "σ");

            // Act
            Pressure value = calcStress;

            // Assert
            Assert.Equal(4.5, value.KilonewtonsPerSquareMeter);
            Assert.Equal(PressureUnit.KilonewtonPerSquareMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcStress1 = new CalcStress(4.5, PressureUnit.Pascal, "σ1", "σ");
            var calcStress2 = new CalcStress(5.5 / 1000, PressureUnit.Kilopascal, "σ2", "σ");

            // Act
            CalcStress result = calcStress1 + calcStress2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("σ", result.Symbol);
            Assert.Equal("Pa", result.Unit);
            Assert.Equal("σ1 + σ2", result.DisplayName);
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Pascal, "σ1", "σ");

            // Act
            CalcStress result = -calcStress;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("σ", result.Symbol);
            Assert.Equal("Pa", result.Unit);
            Assert.Equal("-σ1", result.DisplayName);
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcStress1 = new CalcStress(4.5, PressureUnit.Pascal, "σ1", "σ");
            var calcStress2 = new CalcStress(5.5 / 1000, PressureUnit.Kilopascal, "σ2", "σ");

            // Act
            CalcStress result = calcStress1 - calcStress2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("σ", result.Symbol);
            Assert.Equal("Pa", result.Unit);
            Assert.Equal("σ1 - σ2", result.DisplayName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.Kilopascal, "σ", "σ");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcLinearForce result = calcLength * calcStress; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("σ · l", result.DisplayName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest2()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "σ", "σ");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcLinearForce result = calcStress * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5 * 1000, result.Value, 12);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("N/mm", result.Unit);
            Assert.Equal("σ · l", result.DisplayName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest3()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.KilonewtonPerSquareCentimeter, "σ", "σ");
            var calcLength = new CalcLength(5.5, LengthUnit.Centimeter, "l", "L");

            // Act
            CalcLinearForce result = calcStress * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value, 12);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/cm", result.Unit);
            Assert.Equal("σ · l", result.DisplayName);
        }

        [Fact]
        public void MultiplicationAreaOperatorTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.Kilopascal, "σ", "σ");
            var calcArea = new CalcArea(5.5, AreaUnit.SquareMeter, "A", "A");

            // Act
            CalcForce result = calcArea * calcStress; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN", result.Unit);
            Assert.Equal("σ · A", result.DisplayName);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcStress1 = new CalcStress(4.5, PressureUnit.Kilopascal, "σ1", "σ");
            var calcStress2 = new CalcStress(5.5, PressureUnit.Kilopascal, "σ2", "σ");

            // Act
            CalcDouble result = calcStress1 / calcStress2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("σ1 / σ2", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = 2.0 + calcStress;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = calcStress - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = 2.0 * calcStress;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = calcStress / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }


        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcStress1 = new CalcStress(1, PressureUnit.NewtonPerSquareCentimeter, "a", "A");
            var calcStress2 = new CalcStress(2, PressureUnit.NewtonPerSquareCentimeter, "a", "A");
            var calcStress3 = new CalcStress(3, PressureUnit.NewtonPerSquareCentimeter, "a", "A");
            var areas = new List<CalcStress>() { calcStress1, calcStress2, calcStress3 };

            // Act
            CalcStress sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("N/cm²", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcStress1 = new CalcStress(1, PressureUnit.NewtonPerSquareCentimeter, "a", "A");
            var calcStress2 = new CalcStress(2, PressureUnit.NewtonPerSquareCentimeter, "a", "A");
            var calcStress3 = new CalcStress(3, PressureUnit.NewtonPerSquareCentimeter, "a", "A");
            var areas = new List<CalcStress>() { calcStress1, calcStress2, calcStress3 };

            // Act
            CalcStress sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("N/cm²", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStress1 = new CalcStress(val1, PressureUnit.NewtonPerSquareMeter, "q2", "Q");
            var calcStress2 = new CalcStress(val2 / 10000, PressureUnit.NewtonPerSquareCentimeter, "q2", "Q");

            // Act
            bool result = calcStress1 == calcStress2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStress1 = new CalcStress(val1, PressureUnit.NewtonPerSquareMeter, "q2", "Q");
            var calcStress2 = new CalcStress(val2 / 10000, PressureUnit.NewtonPerSquareCentimeter, "q2", "Q");

            // Act
            bool result = calcStress1 != calcStress2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStress1 = new CalcStress(val1, PressureUnit.NewtonPerSquareMeter, "q2", "Q");
            var calcStress2 = new CalcStress(val2 / 10000, PressureUnit.NewtonPerSquareCentimeter, "q2", "Q");

            // Act
            bool result = calcStress1 > calcStress2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStress1 = new CalcStress(val1, PressureUnit.NewtonPerSquareMeter, "q2", "Q");
            var calcStress2 = new CalcStress(val2 / 10000, PressureUnit.NewtonPerSquareCentimeter, "q2", "Q");

            // Act
            bool result = calcStress1 < calcStress2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, true)]
        [InlineData(4.3, 4.31, false)]
        public void GreaterOrEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStress1 = new CalcStress(val1, PressureUnit.NewtonPerSquareMeter, "q2", "Q");
            var calcStress2 = new CalcStress(val2 / 10000, PressureUnit.NewtonPerSquareCentimeter, "q2", "Q");

            // Act
            bool result = calcStress1 >= calcStress2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.3, 4.31, true)]
        [InlineData(4.31, 4.30, false)]
        public void LessOrEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStress1 = new CalcStress(val1, PressureUnit.NewtonPerSquareMeter, "q2", "Q");
            var calcStress2 = new CalcStress(val2 / 10000, PressureUnit.NewtonPerSquareCentimeter, "q2", "Q");

            // Act
            bool result = calcStress1 <= calcStress2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStress.Equals((object)calcStress));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcStress.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcStress1 = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");
            var calcStress2 = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStress1.Equals((object)calcStress2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");
            var notCalcStress = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcStress.Equals(notCalcStress));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStress.Equals(calcStress));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcStress.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcStress1 = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");
            var calcStress2 = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStress1.Equals(calcStress2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcStress1 = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");
            var calcStress2 = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");
            var calcStress3 = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcStress1.GetHashCode() == calcStress2.GetHashCode();
            bool firstEqualsThird = calcStress1.GetHashCode() == calcStress3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "myQuantity", "Q");

            // Act
            string value = calcStress.GetValueAsString();

            // Assert
            Assert.Equal("4.5 psi", value);
        }
    }
}
