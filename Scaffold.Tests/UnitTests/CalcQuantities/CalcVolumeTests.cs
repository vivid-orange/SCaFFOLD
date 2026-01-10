using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcVolumeTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "volume", "V");

            // Act
            // Assert
            Assert.True(CalcVolume.TryParse("5.5 cm^3", null, out calcVolume));
            Assert.Equal(5.5, calcVolume.Value);
            Assert.Equal("cm³", calcVolume.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcVolume = CalcVolume.Parse("5.5 cm³", null);

            // Assert
            Assert.Equal(5.5, calcVolume.Value);
            Assert.Equal("cm³", calcVolume.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcVolume(4.5, VolumeUnit.CubicFoot, "volume", "V");

            // Act
            // Assert
            Assert.False(CalcVolume.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcVolume = new Volume(4.5, VolumeUnit.CubicMeter);

            // Act
            CalcVolume value = calcVolume;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.TypeName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicInch, "volume", "V");

            // Act
            double value = calcVolume;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicYard, "volume", "V");

            // Act
            Volume value = calcVolume;

            // Assert
            Assert.Equal(4.5, value.CubicYards);
            Assert.Equal(VolumeUnit.CubicYard, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v1", "V");
            var calcVolume2 = new CalcVolume(5.5e-6, VolumeUnit.CubicMeter, "v2", "V");

            // Act
            CalcVolume result = calcVolume1 + calcVolume2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("V", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("v1 + v2", result.TypeName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "V");
            var calcVolume2 = new CalcVolume(5.5e-6, VolumeUnit.CubicMeter, "a2", "V");

            // Act
            CalcVolume result = calcVolume1 - calcVolume2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("V", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1 - a2", result.TypeName); // note: using Thin Space \u2009
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v1", "V");

            // Act
            CalcVolume result = -calcVolume;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("V", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("-v1", result.TypeName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a", "V");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcInertia result = calcVolume * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a · l", result.TypeName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v1", "V");
            var calcVolume2 = new CalcVolume(5.5, VolumeUnit.CubicCentimeter, "v2", "V");

            // Act
            CalcDouble result = calcVolume1 / calcVolume2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("v1 / v2", result.TypeName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v", "V");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcArea result = calcVolume / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("v / l", result.TypeName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm²", result.Unit);
        }

        [Fact]
        public void DivisionAreaOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v", "V");
            var calcArea = new CalcArea(0.00055, AreaUnit.SquareMeter, "a", "A");

            // Act
            CalcLength result = calcVolume / calcArea;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("v / a", result.TypeName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = 2.0 + calcVolume;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcVolume.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = calcVolume - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcVolume.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = 2.0 * calcVolume;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcVolume.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = calcVolume / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcVolume.Value);
        }


        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(1, VolumeUnit.CubicCentimeter, "a", "A");
            var calcVolume2 = new CalcVolume(2, VolumeUnit.CubicCentimeter, "a", "A");
            var calcVolume3 = new CalcVolume(3, VolumeUnit.CubicCentimeter, "a", "A");
            var areas = new List<CalcVolume>() { calcVolume1, calcVolume2, calcVolume3 };

            // Act
            CalcVolume sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("cm³", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(1, VolumeUnit.CubicCentimeter, "a", "A");
            var calcVolume2 = new CalcVolume(2, VolumeUnit.CubicCentimeter, "a", "A");
            var calcVolume3 = new CalcVolume(3, VolumeUnit.CubicCentimeter, "a", "A");
            var areas = new List<CalcVolume>() { calcVolume1, calcVolume2, calcVolume3 };

            // Act
            CalcVolume sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("cm³", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcVolume1 = new CalcVolume(val1, VolumeUnit.CubicMeter, "q2", "Q");
            var calcVolume2 = new CalcVolume(val2 * 1000000, VolumeUnit.CubicCentimeter, "q2", "Q");

            // Act
            bool result = calcVolume1 == calcVolume2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcVolume1 = new CalcVolume(val1, VolumeUnit.CubicMeter, "q2", "Q");
            var calcVolume2 = new CalcVolume(val2 * 1000000, VolumeUnit.CubicCentimeter, "q2", "Q");

            // Act
            bool result = calcVolume1 != calcVolume2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcVolume1 = new CalcVolume(val1, VolumeUnit.CubicMeter, "q2", "Q");
            var calcVolume2 = new CalcVolume(val2 * 1000000, VolumeUnit.CubicCentimeter, "q2", "Q");

            // Act
            bool result = calcVolume1 > calcVolume2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcVolume1 = new CalcVolume(val1, VolumeUnit.CubicMeter, "q2", "Q");
            var calcVolume2 = new CalcVolume(val2 * 1000000, VolumeUnit.CubicCentimeter, "q2", "Q");

            // Act
            bool result = calcVolume1 < calcVolume2;

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
            var calcVolume1 = new CalcVolume(val1, VolumeUnit.CubicMeter, "q2", "Q");
            var calcVolume2 = new CalcVolume(val2 * 1000000, VolumeUnit.CubicCentimeter, "q2", "Q");

            // Act
            bool result = calcVolume1 >= calcVolume2;

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
            var calcVolume1 = new CalcVolume(val1, VolumeUnit.CubicMeter, "q2", "Q");
            var calcVolume2 = new CalcVolume(val2 * 1000000, VolumeUnit.CubicCentimeter, "q2", "Q");

            // Act
            bool result = calcVolume1 <= calcVolume2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcVolume.Equals((object)calcVolume));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcVolume.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");
            var calcVolume2 = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcVolume1.Equals((object)calcVolume2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");
            var notCalcVolume = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcVolume.Equals(notCalcVolume));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcVolume.Equals(calcVolume));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcVolume.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");
            var calcVolume2 = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcVolume1.Equals(calcVolume2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");
            var calcVolume2 = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");
            var calcVolume3 = new CalcVolume(4.5, VolumeUnit.CubicFoot, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcVolume1.GetHashCode() == calcVolume2.GetHashCode();
            bool firstEqualsThird = calcVolume1.GetHashCode() == calcVolume3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "myQuantity", "Q");

            // Act
            string value = calcVolume.GetValueAsString();

            // Assert
            Assert.Equal("4.5 ft³", value);
        }
    }
}
