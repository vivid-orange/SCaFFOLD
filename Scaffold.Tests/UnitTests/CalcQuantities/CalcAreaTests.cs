using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Exceptions;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcAreaTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(CalcArea.TryParse("5.5 cm²", null, out calcArea));
            Assert.Equal(5.5, calcArea.Value);
            Assert.Equal("cm²", calcArea.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcArea = CalcArea.Parse("5.5 cm²", null);

            // Assert
            Assert.Equal(5.5, calcArea.Value);
            Assert.Equal("cm²", calcArea.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.False(CalcArea.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcArea = new Area(4.5, AreaUnit.SquareMeter);

            // Act
            CalcArea value = calcArea;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareInch, "area", "A");

            // Act
            double value = calcArea;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareYard, "area", "A");

            // Act
            Area value = calcArea;

            // Assert
            Assert.Equal(4.5, value.SquareYards);
            Assert.Equal(AreaUnit.SquareYard, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(0.00055, AreaUnit.SquareMeter, "a2", "A");

            // Act
            CalcArea result = calcArea1 + calcArea2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1 + a2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(0.00055, AreaUnit.SquareMeter, "a2", "A");

            // Act
            CalcArea result = calcArea1 - calcArea2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1 - a2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = -calcArea;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("-a1", result.DisplayName);
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(0.00055, AreaUnit.SquareMeter, "a2", "A");

            // Act
            CalcInertia result = calcArea1 * calcArea2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1 · a2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a", "A");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcVolume result = calcArea * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a · l", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(5.5, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            CalcDouble result = calcArea1 / calcArea2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("a1 / a2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a", "A");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcLength result = calcArea / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("a / l", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = 2.0 + calcArea;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = calcArea - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = 2.0 * calcArea;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = calcArea / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void PowerOperatorIntTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcInertia result = (CalcInertia)(calcArea ^ 2);

            // Assert
            Assert.Equal(Math.Pow(4.5, 2), result.Value);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1²", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void PowerOperatorDoubleTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcInertia result = (CalcInertia)(calcArea ^ 2.0);

            // Assert
            Assert.Equal(Math.Pow(4.5, 2), result.Value);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1²", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void PowerOperatorAsSqrtTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcLength result = (CalcLength)(calcArea ^ 0.5);

            // Assert
            Assert.Equal(Math.Pow(4.5, 0.5), result.Value);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("√a1", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void PowerOperatorIntExceptionTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            // Assert
            Assert.Throws<MathException>(() => calcArea ^ 5);
        }

        [Fact]
        public void PowerOperatorDoubleExceptionTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            // Assert
            Assert.Throws<MathException>(() => calcArea ^ 0.4);
        }

        [Fact]
        public void RootTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a", "A");

            // Act
            CalcLength result = calcArea.Sqrt();

            // Assert
            Assert.Equal(Math.Sqrt(4.5), result.Value);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("√a", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(1, AreaUnit.SquareCentimeter, "a", "A");
            var calcArea2 = new CalcArea(2, AreaUnit.SquareCentimeter, "a", "A");
            var calcArea3 = new CalcArea(3, AreaUnit.SquareCentimeter, "a", "A");
            var areas = new List<CalcArea>() { calcArea1, calcArea2, calcArea3 };

            // Act
            CalcArea sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("cm²", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(1, AreaUnit.SquareCentimeter, "a", "A");
            var calcArea2 = new CalcArea(2, AreaUnit.SquareCentimeter, "a", "A");
            var calcArea3 = new CalcArea(3, AreaUnit.SquareCentimeter, "a", "A");
            var areas = new List<CalcArea>() { calcArea1, calcArea2, calcArea3 };

            // Act
            CalcArea sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("cm²", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "q2", "Q");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "q2", "Q");

            // Act
            bool result = calcArea1 == calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "q2", "Q");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "q2", "Q");

            // Act
            bool result = calcArea1 != calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "q2", "Q");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "q2", "Q");

            // Act
            bool result = calcArea1 > calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "q2", "Q");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "q2", "Q");

            // Act
            bool result = calcArea1 < calcArea2;

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
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "q2", "Q");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "q2", "Q");

            // Act
            bool result = calcArea1 >= calcArea2;

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
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "q2", "Q");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "q2", "Q");

            // Act
            bool result = calcArea1 <= calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea.Equals((object)calcArea));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcArea.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");
            var calcArea2 = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea1.Equals((object)calcArea2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");
            var notCalcArea = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcArea.Equals(notCalcArea));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea.Equals(calcArea));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcArea.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");
            var calcArea2 = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea1.Equals(calcArea2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");
            var calcArea2 = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");
            var calcArea3 = new CalcArea(4.5, AreaUnit.SquareFoot, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcArea1.GetHashCode() == calcArea2.GetHashCode();
            bool firstEqualsThird = calcArea1.GetHashCode() == calcArea3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "myQuantity", "Q");

            // Act
            string value = calcArea.GetValueAsString();

            // Assert
            Assert.Equal("4.5 ft²", value);
        }
    }
}
