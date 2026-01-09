using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcIntTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcInt = new CalcInt(4);

            // Act
            // Assert
            Assert.True(calcInt.TryParse("5"));
            Assert.Equal(5, calcInt.Value);
        }

        [Fact]
        public void ImplicitFromWrappedTypeTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);

            // Act
            calcInt1 = 5;
            CalcInt calcInt2 = 2;

            // Assert
            Assert.Equal(5, calcInt1.Value);
            Assert.Equal(2, calcInt2.Value);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);

            // Act
            int result = calcInt1;

            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public void Constructor_WithValueAndDisplayName_SetsProperties()
        {
            // Arrange & Act
            var calcInt = new CalcInt(4, "test value");

            // Assert
            Assert.Equal(4, calcInt.Value);
            Assert.Equal("test value", calcInt.DisplayName);
            Assert.True(string.IsNullOrEmpty(calcInt.Symbol));
            Assert.True(string.IsNullOrEmpty(calcInt.Unit));
        }

        [Fact]
        public void Constructor_WithValueDisplayNameAndSymbol_SetsProperties()
        {
            // Arrange & Act
            var calcInt = new CalcInt(4, "test value", "n");

            // Assert
            Assert.Equal(4, calcInt.Value);
            Assert.Equal("test value", calcInt.DisplayName);
            Assert.Equal("n", calcInt.Symbol);
            Assert.True(string.IsNullOrEmpty(calcInt.Unit));
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, false)]
        public void EqualOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcInt2 = new CalcInt(val2);

            // Act
            bool result = calcInt1 == calcInt2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, false)]
        public void EqualIntOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);

            // Act
            bool result = calcInt1 == val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4.0, true)]
        [InlineData(4, 4.5, false)]
        public void EqualCalcDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);
            var calcDouble = new CalcDouble(val2);

            // Act
            bool result = calcInt == calcDouble;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4.0, true)]
        [InlineData(4, 4.5, false)]
        public void EqualDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);

            // Act
            bool result = calcInt == val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, false)]
        [InlineData(4, 5, true)]
        public void NotEqualOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcInt2 = new CalcInt(val2);

            // Act
            bool result = calcInt1 != calcInt2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, false)]
        [InlineData(4, 5, true)]
        public void NotEqualIntOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcInt2 = new CalcInt(val2);

            // Act
            bool result = calcInt1 != calcInt2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4.0, false)]
        [InlineData(4, 4.5, true)]
        public void NotEqualCalcDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcDouble = new CalcDouble(val2);

            // Act
            bool result = calcInt1 != calcDouble;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4.0, false)]
        [InlineData(4, 4.5, true)]
        public void NotEqualDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);

            // Act
            bool result = calcInt1 != val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2, true)] // 5 > 2
        [InlineData(2, 5, false)] // 2 > 5
        [InlineData(5, 5, false)] // 5 > 5
        public void GreaterThanOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcInt2 = new CalcInt(val2);

            // Act
            bool result = calcInt1 > calcInt2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2, true)] // 5 > 2
        [InlineData(2, 5, false)] // 2 > 5
        [InlineData(5, 5, false)] // 5 > 5
        public void GreaterThanIntOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);

            // Act
            bool result = calcInt1 > val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2.0, true)] // 5 > 2
        [InlineData(2, 5.0, false)] // 2 > 5
        [InlineData(5, 5.0, false)] // 5 > 5
        public void GreaterThanCalcDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);
            var calcDouble = new CalcDouble(val2);

            // Act
            bool result = calcInt > calcDouble;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2.0, true)] // 5 > 2
        [InlineData(2, 5.0, false)] // 2 > 5
        [InlineData(5, 5.0, false)] // 5 > 5
        public void GreaterThanDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);
            var calcDouble = new CalcDouble(val2);

            // Act
            bool result = calcInt > val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2, false)] // 5 < 2
        [InlineData(2, 5, true)] // 2 < 5
        [InlineData(5, 5, false)] // 5 < 5
        public void LessThanOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcInt2 = new CalcInt(val2);

            // Act
            bool result = calcInt1 < calcInt2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2, false)] // 5 < 2
        [InlineData(2, 5, true)] // 2 < 5
        [InlineData(5, 5, false)] // 5 < 5
        public void LessThanIntOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);

            // Act
            bool result = calcInt1 < val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2, false)] // 5 < 2
        [InlineData(2, 5, true)] // 2 < 5
        [InlineData(5, 5, false)] // 5 < 5
        public void LessThanCalcDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);
            var calcDouble = new CalcDouble(val2);

            // Act
            bool result = calcInt < calcDouble;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 2.0, false)] // 5 < 2
        [InlineData(2, 5.0, true)] // 2 < 5
        [InlineData(5, 5.0, false)] // 5 < 5
        public void LessThanDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);

            // Act
            bool result = calcInt < val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(5, 4, true)]
        [InlineData(4, 5, false)]
        public void GreaterOrEqualOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcInt2 = new CalcInt(val2);

            // Act
            bool result = calcInt1 >= calcInt2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(5, 4, true)]
        [InlineData(4, 5, false)]
        public void GreaterOrEqualIntOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);

            // Act
            bool result = calcInt1 >= val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4.0, true)]
        [InlineData(5, 4.0, true)]
        [InlineData(4, 5.0, false)]
        public void GreaterOrEqualCalcDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);
            var calcDouble = new CalcDouble(val2);

            // Act
            bool result = calcInt >= calcDouble;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4.0, true)]
        [InlineData(5, 4.0, true)]
        [InlineData(4, 5.0, false)]
        public void GreaterOrEqualDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);

            // Act
            bool result = calcInt >= val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, true)]
        [InlineData(5, 4, false)]
        public void LessOrEqualOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);
            var calcInt2 = new CalcInt(val2);

            // Act
            bool result = calcInt1 <= calcInt2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, true)]
        [InlineData(5, 4, false)]
        public void LessOrEqualIntOperatorTest(int val1, int val2, bool expected)
        {
            // Arrange
            var calcInt1 = new CalcInt(val1);

            // Act
            bool result = calcInt1 <= val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, true)]
        [InlineData(5, 4, false)]
        public void LessOrEqualCalcDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);
            var calcDouble = new CalcDouble(val2);

            // Act
            bool result = calcInt <= calcDouble;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, true)]
        [InlineData(5, 4, false)]
        public void LessOrEqualDoubleOperatorTest(int val1, double val2, bool expected)
        {
            // Arrange
            var calcInt = new CalcInt(val1);

            // Act
            bool result = calcInt <= val2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 + calcInt2;

            // Assert
            Assert.Equal(9, result.Value);
        }

        [Fact]
        public void AdditionSameUnitOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4, "n1", "Some stuff", "some");
            var calcInt2 = new CalcInt(5, "n2", "Some stuff", "some");

            // Act
            CalcInt result = calcInt1 + calcInt2;

            // Assert
            Assert.Equal(9, result.Value);
            Assert.Equal("Some stuff", result.Symbol);
            Assert.Equal("some", result.Unit);
            Assert.Equal("n1 + n2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 - calcInt2;

            // Assert
            Assert.Equal(-1.0, result.Value);
        }

        [Fact]
        public void SubtractionSameUnitOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4, "n1", "Some stuff", "some");
            var calcInt2 = new CalcInt(5, "n2", "Some stuff", "some");

            // Act
            CalcInt result = calcInt1 - calcInt2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("Some stuff", result.Symbol);
            Assert.Equal("some", result.Unit);
            Assert.Equal("n1 - n2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 * calcInt2;

            // Assert
            Assert.Equal(4 * 5, result.Value);
        }

        [Fact]
        public void MultiplicationIntOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);

            // Act
            CalcInt result = 5 * calcInt1;

            // Assert
            Assert.Equal(4 * 5, result.Value);
        }

        [Fact]
        public void MultiplicationSameUnitOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4, "n1", "Some stuff", "some");
            var calcInt2 = new CalcInt(5, "n2", "Some stuff", "some");

            // Act
            CalcInt result = calcInt1 * calcInt2;

            // Assert
            Assert.Equal(4 * 5, result.Value);
            Assert.Equal("Some stuff", result.Symbol);
            Assert.Equal("some²", result.Unit);
            Assert.Equal("n1 · n2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 / calcInt2;

            // Assert
            Assert.Equal(4 / 5, result.Value);
        }

        [Fact]
        public void DivisionIntOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 / 5;

            // Assert
            Assert.Equal(4 / 5, result.Value);
        }

        [Fact]
        public void DivisionSameUnitOperatorTest()
        {
            // Arrange
            var calcInt1 = new CalcInt(4, "n1", "Some stuff", "some");
            var calcInt2 = new CalcInt(5, "n2", "Some stuff", "some");

            // Act
            CalcInt result = calcInt1 / calcInt2;

            // Assert
            Assert.Equal(4 / 5, result.Value);
            Assert.Equal("Some stuff", result.Symbol);
            Assert.True(string.IsNullOrEmpty(result.Unit));
            Assert.Equal("n1 / n2", result.DisplayName); // note: using Thin Space \u2009
        }
    }
}
