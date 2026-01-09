using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcCurvatureTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseMeter, "length", "L");

            // Act
            // Assert
            Assert.True(CalcCurvature.TryParse("5 cm⁻¹", null, out calcLength));
            Assert.Equal(5, calcLength.Value);
            Assert.Equal("cm⁻¹", calcLength.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcLength = CalcCurvature.Parse("5.5 cm⁻¹", null);

            // Assert
            Assert.Equal(5.5, calcLength.Value);
            Assert.Equal("cm⁻¹", calcLength.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseMeter, "length", "L");

            // Act
            // Assert
            Assert.False(CalcCurvature.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcCurvature = new ReciprocalLength(4.5, ReciprocalLengthUnit.InverseCentimeter);

            // Act
            CalcCurvature value = calcCurvature;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseMeter, "length", "L");

            // Act
            double value = calcLength;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseMeter, "length", "L");

            // Act
            ReciprocalLength value = calcLength;

            // Assert
            Assert.Equal(4.5, value.InverseMeters);
            Assert.Equal(ReciprocalLengthUnit.InverseMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "l1", "L");
            var calcLength2 = new CalcCurvature(550, ReciprocalLengthUnit.InverseMeter, "l2", "L");

            // Act
            CalcCurvature result = calcLength1 + calcLength2;

            // Assert
            Assert.Equal(4.5 + 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "l1", "L");
            var calcLength2 = new CalcCurvature(550, ReciprocalLengthUnit.InverseMeter, "l2", "L");

            // Act
            CalcCurvature result = calcLength1 - calcLength2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("l1 - l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "a1", "A");

            // Act
            CalcCurvature result = -calcLength;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("-a1", result.DisplayName);
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Arrange
            var calcCurvature = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "1/l1", "1/L");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcDouble result = calcCurvature * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.Equal("-", result.Unit);
            Assert.Equal("1/l1 · l2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "l1", "L");
            var calcLength2 = new CalcCurvature(5.5, ReciprocalLengthUnit.InverseCentimeter, "l2", "L");

            // Act
            CalcDouble result = calcLength1 / calcLength2;

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
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "a1", "A");

            // Act
            CalcCurvature result = 2.0 + calcLength;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "a1", "A");

            // Act
            CalcCurvature result = calcLength - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "a1", "A");

            // Act
            CalcCurvature result = 2.0 * calcLength;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseCentimeter, "a1", "A");

            // Act
            CalcCurvature result = calcLength / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(1, ReciprocalLengthUnit.InverseCentimeter, "a", "A");
            var calcLength2 = new CalcCurvature(2, ReciprocalLengthUnit.InverseCentimeter, "a", "A");
            var calcLength3 = new CalcCurvature(3, ReciprocalLengthUnit.InverseCentimeter, "a", "A");
            var areas = new List<CalcCurvature>() { calcLength1, calcLength2, calcLength3 };

            // Act
            CalcCurvature sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("cm⁻¹", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(1, ReciprocalLengthUnit.InverseCentimeter, "a", "A");
            var calcLength2 = new CalcCurvature(2, ReciprocalLengthUnit.InverseCentimeter, "a", "A");
            var calcLength3 = new CalcCurvature(3, ReciprocalLengthUnit.InverseCentimeter, "a", "A");
            var areas = new List<CalcCurvature>() { calcLength1, calcLength2, calcLength3 };

            // Act
            CalcCurvature sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("cm⁻¹", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLength1 = new CalcCurvature(val1, ReciprocalLengthUnit.InverseMeter, "q2", "Q");
            var calcLength2 = new CalcCurvature(val2 / 100, ReciprocalLengthUnit.InverseCentimeter, "q2", "Q");

            // Act
            bool result = calcLength1 == calcLength2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLength1 = new CalcCurvature(val1, ReciprocalLengthUnit.InverseMeter, "q2", "Q");
            var calcLength2 = new CalcCurvature(val2 / 100, ReciprocalLengthUnit.InverseCentimeter, "q2", "Q");

            // Act
            bool result = calcLength1 != calcLength2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLength1 = new CalcCurvature(val1, ReciprocalLengthUnit.InverseMeter, "q2", "Q");
            var calcLength2 = new CalcCurvature(val2 / 100, ReciprocalLengthUnit.InverseCentimeter, "q2", "Q");

            // Act
            bool result = calcLength1 > calcLength2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLength1 = new CalcCurvature(val1, ReciprocalLengthUnit.InverseMeter, "q2", "Q");
            var calcLength2 = new CalcCurvature(val2 / 100, ReciprocalLengthUnit.InverseCentimeter, "q2", "Q");

            // Act
            bool result = calcLength1 < calcLength2;

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
            var calcLength1 = new CalcCurvature(val1, ReciprocalLengthUnit.InverseMeter, "q2", "Q");
            var calcLength2 = new CalcCurvature(val2 / 100, ReciprocalLengthUnit.InverseCentimeter, "q2", "Q");

            // Act
            bool result = calcLength1 >= calcLength2;

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
            var calcLength1 = new CalcCurvature(val1, ReciprocalLengthUnit.InverseMeter, "q2", "Q");
            var calcLength2 = new CalcCurvature(val2 / 100, ReciprocalLengthUnit.InverseCentimeter, "q2", "Q");

            // Act
            bool result = calcLength1 <= calcLength2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength.Equals((object)calcLength));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLength.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");
            var calcLength2 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength1.Equals((object)calcLength2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");
            var notCalcCurvature = new CalcArea(4.5, AreaUnit.SquareFoot, "length", "l");

            // Act
            // Assert
            Assert.False(calcLength.Equals(notCalcCurvature));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength.Equals(calcLength));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLength.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");
            var calcLength2 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength1.Equals(calcLength2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcLength1 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");
            var calcLength2 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");
            var calcLength3 = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcLength1.GetHashCode() == calcLength2.GetHashCode();
            bool firstEqualsThird = calcLength1.GetHashCode() == calcLength3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcLength = new CalcCurvature(4.5, ReciprocalLengthUnit.InverseFoot, "myQuantity", "Q");

            // Act
            string value = calcLength.GetValueAsString();

            // Assert
            Assert.Equal("4.5 ft⁻¹", value);
        }
    }
}
