using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Exceptions;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcLengthTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            // Assert
            Assert.True(CalcLength.TryParse("5.5 cm", null, out calcLength));
            Assert.Equal(5.5, calcLength.Value);
            Assert.Equal("cm", calcLength.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcLength = CalcLength.Parse("5.5 cm", null);

            // Assert
            Assert.Equal(5.5, calcLength.Value);
            Assert.Equal("cm", calcLength.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            // Assert
            Assert.False(CalcLength.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcLength = new Length(4.5, LengthUnit.Megameter);

            // Act
            CalcLength value = calcLength;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            double value = calcLength;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            Length value = calcLength;

            // Assert
            Assert.Equal(4.5, value.Meters);
            Assert.Equal(LengthUnit.Meter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcLength result = calcLength1 + calcLength2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcLength result = calcLength1 - calcLength2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("l1 - l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = -calcLength;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("-a1", result.DisplayName);
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcArea result = calcLength1 * calcLength2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("l1 · l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationAreaOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "l", "L");
            var calcArea = new CalcArea(5.5, AreaUnit.SquareCentimeter, "a", "A");

            // Act
            CalcVolume result = calcLength * calcArea;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("l · a", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationVolumeOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "l", "L");
            var calcArea = new CalcVolume(5.5, VolumeUnit.CubicCentimeter, "v", "V");

            // Act
            CalcInertia result = calcLength * calcArea;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("l · v", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationStrainOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "l", "L");
            var calcStrain = new CalcStrain(0.05, StrainUnit.DecimalFraction, "v", "V");

            // Act
            CalcLength result = calcLength * calcStrain;

            // Assert
            Assert.Equal(4.5 * 0.05, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.DisplayName));
            Assert.Equal("cm", result.Unit);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(5.5, LengthUnit.Centimeter, "l2", "L");

            // Act
            CalcStrain result = calcLength1 / calcLength2;

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
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = 2.0 + calcLength;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = calcLength - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = 2.0 * calcLength;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = calcLength / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void DivisionInverseOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcCurvature result = 1 / calcLength;

            // Assert
            Assert.Equal(1 / 4.5, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("1/a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void PowerOperatorMinus1Test()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcCurvature result = (CalcCurvature)(calcLength ^ -1);

            // Assert
            Assert.Equal(Math.Pow(4.5, -1), result.Value);
            Assert.Equal("cm⁻¹", result.Unit);
            Assert.Equal("1/a1", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void PowerOperator2Test()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcArea result = (CalcArea)(calcLength ^ 2);

            // Assert
            Assert.Equal(Math.Pow(4.5, 2), result.Value);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1²", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void PowerOperator3Test()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcVolume result = (CalcVolume)(calcLength ^ 3);

            // Assert
            Assert.Equal(Math.Pow(4.5, 3), result.Value);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1³", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void PowerOperator4Test()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcInertia result = (CalcInertia)(calcLength ^ 4);

            // Assert
            Assert.Equal(Math.Pow(4.5, 4), result.Value);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1⁴", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void PowerOperatorDoubleExceptionTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            // Assert
            Assert.Throws<MathException>(() => calcLength ^ 0.4);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(1, LengthUnit.Centimeter, "a", "A");
            var calcLength2 = new CalcLength(2, LengthUnit.Centimeter, "a", "A");
            var calcLength3 = new CalcLength(3, LengthUnit.Centimeter, "a", "A");
            var areas = new List<CalcLength>() { calcLength1, calcLength2, calcLength3 };

            // Act
            CalcLength sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("cm", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(1, LengthUnit.Centimeter, "a", "A");
            var calcLength2 = new CalcLength(2, LengthUnit.Centimeter, "a", "A");
            var calcLength3 = new CalcLength(3, LengthUnit.Centimeter, "a", "A");
            var areas = new List<CalcLength>() { calcLength1, calcLength2, calcLength3 };

            // Act
            CalcLength sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("cm", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLength1 = new CalcLength(val1, LengthUnit.Meter, "q2", "Q");
            var calcLength2 = new CalcLength(val2 * 100, LengthUnit.Centimeter, "q2", "Q");

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
            var calcLength1 = new CalcLength(val1, LengthUnit.Meter, "q2", "Q");
            var calcLength2 = new CalcLength(val2 * 100, LengthUnit.Centimeter, "q2", "Q");

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
            var calcLength1 = new CalcLength(val1, LengthUnit.Meter, "q2", "Q");
            var calcLength2 = new CalcLength(val2 * 100, LengthUnit.Centimeter, "q2", "Q");

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
            var calcLength1 = new CalcLength(val1, LengthUnit.Meter, "q2", "Q");
            var calcLength2 = new CalcLength(val2 * 100, LengthUnit.Centimeter, "q2", "Q");

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
            var calcLength1 = new CalcLength(val1, LengthUnit.Meter, "q2", "Q");
            var calcLength2 = new CalcLength(val2 * 100, LengthUnit.Centimeter, "q2", "Q");

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
            var calcLength1 = new CalcLength(val1, LengthUnit.Meter, "q2", "Q");
            var calcLength2 = new CalcLength(val2 * 100, LengthUnit.Centimeter, "q2", "Q");

            // Act
            bool result = calcLength1 <= calcLength2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength.Equals((object)calcLength));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLength.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");
            var calcLength2 = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength1.Equals((object)calcLength2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");
            var notCalcLength = new CalcArea(4.5, AreaUnit.SquareFoot, "length", "l");

            // Act
            // Assert
            Assert.False(calcLength.Equals(notCalcLength));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength.Equals(calcLength));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLength.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");
            var calcLength2 = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLength1.Equals(calcLength2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");
            var calcLength2 = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");
            var calcLength3 = new CalcLength(4.5, LengthUnit.Foot, "MyQuantity", "Q");

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
            var calcLength = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            string value = calcLength.GetValue();

            // Assert
            Assert.Equal("4.5 ft", value);
        }

        [Fact]
        public void InverseTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Foot, "myQuantity", "Q");

            // Act
            CalcCurvature value = calcLength.Inverse();

            // Assert
            Assert.Equal(1 / 4.5, value);
            Assert.Equal("ft⁻¹", value.Unit);
        }
    }
}
