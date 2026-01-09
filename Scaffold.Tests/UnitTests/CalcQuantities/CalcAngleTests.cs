using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcAngleTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Degree, "length", "L");

            // Act
            // Assert
            Assert.True(CalcAngle.TryParse("5 rad", null, out calcAngle));
            Assert.Equal(5, calcAngle.Value);
            Assert.Equal("rad", calcAngle.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcAngle = CalcAngle.Parse("5.5 deg", null);

            // Assert
            Assert.Equal(5.5, calcAngle.Value);
            Assert.Equal("°", calcAngle.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcAngle(4.5, AngleUnit.Degree, "length", "L");

            // Act
            // Assert
            Assert.False(CalcAngle.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcAngle = new Angle(4.5, AngleUnit.Degree);

            // Act
            CalcAngle value = calcAngle;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Degree, "length", "L");

            // Act
            double value = calcAngle;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Degree, "length", "L");

            // Act
            Angle value = calcAngle;

            // Assert
            Assert.Equal(4.5, value.Degrees);
            Assert.Equal(AngleUnit.Degree, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(4.5, AngleUnit.Radian, "l1", "L");
            var calcAngle2 = new CalcAngle(5.5 * 180 / Math.PI, AngleUnit.Degree, "l2", "L");

            // Act
            CalcAngle result = calcAngle1 + calcAngle2;

            // Assert
            Assert.Equal(4.5 + 5.5, result.Value, 12);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("rad", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(4.5, AngleUnit.Radian, "l1", "L");
            var calcAngle2 = new CalcAngle(5.5 * 180 / Math.PI, AngleUnit.Degree, "l2", "L");

            // Act
            CalcAngle result = calcAngle1 - calcAngle2;

            // Assert
            Assert.Equal(-1, result.Value, 12);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("rad", result.Unit);
            Assert.Equal("l1 - l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "a1", "A");

            // Act
            CalcAngle result = -calcAngle;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("rad", result.Unit);
            Assert.Equal("-a1", result.DisplayName);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(4.5, AngleUnit.Radian, "l1", "L");
            var calcAngle2 = new CalcAngle(5.5, AngleUnit.Radian, "l2", "L");

            // Act
            CalcDouble result = calcAngle1 / calcAngle2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("l1 / l2", result.DisplayName); // note: using Thin Space \u2009
            Assert.Equal("-", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "a1", "A");

            // Act
            CalcAngle result = 2.0 + calcAngle;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("rad", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcAngle.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "a1", "A");

            // Act
            CalcAngle result = calcAngle - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("rad", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcAngle.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "a1", "A");

            // Act
            CalcAngle result = 2.0 * calcAngle;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("rad", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcAngle.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "a1", "A");

            // Act
            CalcAngle result = calcAngle / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("rad", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcAngle.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(1, AngleUnit.Radian, "a", "A");
            var calcAngle2 = new CalcAngle(2, AngleUnit.Radian, "a", "A");
            var calcAngle3 = new CalcAngle(3, AngleUnit.Radian, "a", "A");
            var areas = new List<CalcAngle>() { calcAngle1, calcAngle2, calcAngle3 };

            // Act
            CalcAngle sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("rad", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(1, AngleUnit.Radian, "a", "A");
            var calcAngle2 = new CalcAngle(2, AngleUnit.Radian, "a", "A");
            var calcAngle3 = new CalcAngle(3, AngleUnit.Radian, "a", "A");
            var areas = new List<CalcAngle>() { calcAngle1, calcAngle2, calcAngle3 };

            // Act
            CalcAngle sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("rad", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcAngle1 = new CalcAngle(val1, AngleUnit.Radian, "q2", "Q");
            var calcAngle2 = new CalcAngle(val2 * 180 / Math.PI, AngleUnit.Degree, "q2", "Q");

            // Act
            bool result = calcAngle1 == calcAngle2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcAngle1 = new CalcAngle(val1, AngleUnit.Radian, "q2", "Q");
            var calcAngle2 = new CalcAngle(val2 * 180 / Math.PI, AngleUnit.Degree, "q2", "Q");

            // Act
            bool result = calcAngle1 != calcAngle2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcAngle1 = new CalcAngle(val1, AngleUnit.Radian, "q2", "Q");
            var calcAngle2 = new CalcAngle(val2 * 180 / Math.PI, AngleUnit.Degree, "q2", "Q");

            // Act
            bool result = calcAngle1 > calcAngle2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcAngle1 = new CalcAngle(val1, AngleUnit.Radian, "q2", "Q");
            var calcAngle2 = new CalcAngle(val2 * 180 / Math.PI, AngleUnit.Degree, "q2", "Q");

            // Act
            bool result = calcAngle1 < calcAngle2;

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
            var calcAngle1 = new CalcAngle(val1, AngleUnit.Radian, "q2", "Q");
            var calcAngle2 = new CalcAngle(val2 * 180 / Math.PI, AngleUnit.Degree, "q2", "Q");

            // Act
            bool result = calcAngle1 >= calcAngle2;

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
            var calcAngle1 = new CalcAngle(val1, AngleUnit.Radian, "q2", "Q");
            var calcAngle2 = new CalcAngle(val2 * 180 / Math.PI, AngleUnit.Degree, "q2", "Q");

            // Act
            bool result = calcAngle1 <= calcAngle2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcAngle.Equals((object)calcAngle));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcAngle.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");
            var calcAngle2 = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcAngle1.Equals((object)calcAngle2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");
            var notCalcAngle = new CalcArea(4.5, AreaUnit.SquareMeter, "length", "l");

            // Act
            // Assert
            Assert.False(calcAngle.Equals(notCalcAngle));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcAngle.Equals(calcAngle));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcAngle.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");
            var calcAngle2 = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcAngle1.Equals(calcAngle2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcAngle1 = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");
            var calcAngle2 = new CalcAngle(4.5, AngleUnit.Radian, "myQuantity", "Q");
            var calcAngle3 = new CalcAngle(4.5, AngleUnit.Radian, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcAngle1.GetHashCode() == calcAngle2.GetHashCode();
            bool firstEqualsThird = calcAngle1.GetHashCode() == calcAngle3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcAngle = new CalcAngle(4.5, AngleUnit.Degree, "myQuantity", "Q");

            // Act
            string value = calcAngle.GetValue();

            // Assert
            Assert.Equal("4.5 °", value);
        }

        [Fact]
        public void FromDegreesTest()
        {
            // Arrange
            // Act
            var calcAngle = CalcAngle.FromDegrees(4.5, "myQuantity", "Q");

            // Assert
            Assert.Equal(4.5, calcAngle.Value);
            Assert.Equal("°", calcAngle.Unit);
            Assert.Equal("myQuantity", calcAngle.DisplayName);
            Assert.Equal("Q", calcAngle.Symbol);
        }

        [Fact]
        public void FromRadiansTest()
        {
            // Arrange
            // Act
            var calcAngle = CalcAngle.FromRadians(4.5, "myQuantity", "Q");

            // Assert
            Assert.Equal(4.5, calcAngle.Value);
            Assert.Equal("rad", calcAngle.Unit);
            Assert.Equal("myQuantity", calcAngle.DisplayName);
            Assert.Equal("Q", calcAngle.Symbol);
        }
    }
}
