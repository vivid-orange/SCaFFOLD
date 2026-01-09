using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Exceptions;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcInertiaTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "inertia", "I");

            // Act
            // Assert
            Assert.True(CalcInertia.TryParse("5.5 cm⁴", null, out calcInertia));
            Assert.Equal(5.5, calcInertia.Value);
            Assert.Equal("cm⁴", calcInertia.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcInertia = CalcInertia.Parse("5.5 cm⁴", null);

            // Assert
            Assert.Equal(5.5, calcInertia.Value);
            Assert.Equal("cm⁴", calcInertia.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "inertia", "I");

            // Act
            // Assert
            Assert.False(CalcInertia.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcInertia = new AreaMomentOfInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth);

            // Act
            CalcInertia value = calcInertia;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.InchToTheFourth, "inertia", "I");

            // Act
            double value = calcInertia;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.DecimeterToTheFourth, "inertia", "I");

            // Act
            AreaMomentOfInertia value = calcInertia;

            // Assert
            Assert.Equal(4.5, value.DecimetersToTheFourth);
            Assert.Equal(AreaMomentOfInertiaUnit.DecimeterToTheFourth, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcInertia1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");
            var calcInertia2 = new CalcInertia(5.5e-8, AreaMomentOfInertiaUnit.MeterToTheFourth, "i2", "I");

            // Act
            CalcInertia result = calcInertia1 + calcInertia2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("i1 + i2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcInertia1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");
            var calcInertia2 = new CalcInertia(5.5e-8, AreaMomentOfInertiaUnit.MeterToTheFourth, "i2", "I");

            // Act
            CalcInertia result = calcInertia1 - calcInertia2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("i1 - i2", result.DisplayName); // note: using Thin Space \u2009
        }


        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcInertia1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");
            var calcInertia2 = new CalcInertia(5.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i2", "I");

            // Act
            CalcDouble result = calcInertia1 / calcInertia2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i1 / i2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i", "I");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcVolume result = calcInertia / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i / l", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm³", result.Unit);
        }

        [Fact]
        public void DivisionAreaOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i", "I");
            var calcArea = new CalcArea(0.00055, AreaUnit.SquareMeter, "a", "A");

            // Act
            CalcArea result = calcInertia / calcArea;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i / a", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm²", result.Unit);
        }

        [Fact]
        public void DivisionVolumeOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i", "I");
            var calcVolume = new CalcVolume(5.5e-6, VolumeUnit.CubicMeter, "v", "V");

            // Act
            CalcLength result = calcInertia / calcVolume;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i / v", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a1", "I");

            // Act
            CalcInertia result = 2.0 + calcInertia;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");

            // Act
            CalcInertia result = calcInertia - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("i1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");

            // Act
            CalcInertia result = -calcInertia;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("-i1", result.DisplayName);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a1", "I");

            // Act
            CalcInertia result = 2.0 * calcInertia;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a1", "I");

            // Act
            CalcInertia result = calcInertia / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void PowerOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");

            // Act
            CalcArea result = (CalcArea)(calcInertia ^ 0.5);

            // Assert
            Assert.Equal(Math.Pow(4.5, 0.5), result.Value);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("√i1", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void PowerOperatorDoubleExceptionTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");

            // Act
            // Assert
            Assert.Throws<MathException>(() => calcInertia ^ 2);
        }

        [Fact]
        public void RootTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");

            // Act
            CalcArea result = calcInertia.Sqrt();

            // Assert
            Assert.Equal(Math.Sqrt(4.5), result.Value);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("√i1", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcArea1 = new CalcInertia(1, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a", "A");
            var calcArea2 = new CalcInertia(2, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a", "A");
            var calcArea3 = new CalcInertia(3, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a", "A");
            var areas = new List<CalcInertia>() { calcArea1, calcArea2, calcArea3 };

            // Act
            CalcInertia sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("cm⁴", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcArea1 = new CalcInertia(1, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a", "A");
            var calcArea2 = new CalcInertia(2, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a", "A");
            var calcArea3 = new CalcInertia(3, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a", "A");
            var areas = new List<CalcInertia>() { calcArea1, calcArea2, calcArea3 };

            // Act
            CalcInertia sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("cm⁴", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcInertia(val1, AreaMomentOfInertiaUnit.MeterToTheFourth, "q2", "Q");
            var calcArea2 = new CalcInertia(val2 * 100000000, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "q2", "Q");

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
            var calcArea1 = new CalcInertia(val1, AreaMomentOfInertiaUnit.MeterToTheFourth, "q2", "Q");
            var calcArea2 = new CalcInertia(val2 * 100000000, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "q2", "Q");

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
            var calcArea1 = new CalcInertia(val1, AreaMomentOfInertiaUnit.MeterToTheFourth, "q2", "Q");
            var calcArea2 = new CalcInertia(val2 * 100000000, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "q2", "Q");

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
            var calcArea1 = new CalcInertia(val1, AreaMomentOfInertiaUnit.MeterToTheFourth, "q2", "Q");
            var calcArea2 = new CalcInertia(val2 * 100000000, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "q2", "Q");

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
            var calcArea1 = new CalcInertia(val1, AreaMomentOfInertiaUnit.MeterToTheFourth, "q2", "Q");
            var calcArea2 = new CalcInertia(val2 * 100000000, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "q2", "Q");

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
            var calcArea1 = new CalcInertia(val1, AreaMomentOfInertiaUnit.MeterToTheFourth, "q2", "Q");
            var calcArea2 = new CalcInertia(val2 * 100000000, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "q2", "Q");

            // Act
            bool result = calcArea1 <= calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcArea = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea.Equals((object)calcArea));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcArea = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcArea.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcArea1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");
            var calcArea2 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea1.Equals((object)calcArea2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcArea = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");
            var notCalcInertia = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcArea.Equals(notCalcInertia));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcArea = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea.Equals(calcArea));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcArea = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcArea.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcArea1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");
            var calcArea2 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcArea1.Equals(calcArea2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcArea1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");
            var calcArea2 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");
            var calcArea3 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "MyQuantity", "Q");

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
            var calcArea = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "myQuantity", "Q");

            // Act
            string value = calcArea.GetValue();

            // Assert
            Assert.Equal("4.5 ft⁴", value);
        }
    }
}
