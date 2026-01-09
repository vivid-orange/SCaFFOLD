using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcLinearMomentTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "moment", "M");

            // Act
            // Assert
            Assert.True(CalcLinearMoment.TryParse("5.5 N·cm/m", null, out calcMoment));
            Assert.Equal(5.5, calcMoment.Value);
            Assert.Equal("N·cm/m", calcMoment.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcMoment = CalcLinearMoment.Parse("5.5 N·cm/m", null);

            // Assert
            Assert.Equal(5.5, calcMoment.Value);
            Assert.Equal("N·cm/m", calcMoment.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "moment", "M");

            // Act
            // Assert
            Assert.False(CalcLinearMoment.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new MomentPerLength(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter);

            // Act
            CalcLinearMoment value = calcLinearMoment;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, "moment", "M");

            // Act
            double value = calcMoment;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, "moment", "M");

            // Act
            MomentPerLength value = calcMoment;

            // Assert
            Assert.Equal(4.5, value.KilonewtonMetersPerMeter);
            Assert.Equal(MomentPerLengthUnit.KilonewtonMeterPerMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcLinearMoment(4.5, MomentPerLengthUnit.NewtonMeterPerMeter, "m1", "M");
            var calcMoment2 = new CalcLinearMoment(5.5 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "m2", "M");

            // Act
            CalcLinearMoment result = calcMoment1 + calcMoment2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m/m", result.Unit);
            Assert.Equal("m1 + m2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcLinearMoment(4.5, MomentPerLengthUnit.NewtonMeterPerMeter, "m1", "M");
            var calcMoment2 = new CalcLinearMoment(5.5 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "m2", "M");

            // Act
            CalcLinearMoment result = calcMoment1 - calcMoment2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m/m", result.Unit);
            Assert.Equal("m1 - m2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "m", "M");

            // Act
            CalcLinearMoment result = -calcMoment;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("-m", result.DisplayName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "m", "M");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcMoment result = calcLength * calcMoment; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("m · l", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcLinearMoment(4.5, "m1", "M");
            var calcMoment2 = new CalcLinearMoment(5.5, "m2", "M");

            // Act
            CalcDouble result = calcMoment1 / calcMoment2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m1 / m2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = 2.0 + calcLinearMoment;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = calcLinearMoment - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = 2.0 * calcLinearMoment;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = calcLinearMoment / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }


        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(1, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a", "A");
            var calcLinearMoment2 = new CalcLinearMoment(2, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a", "A");
            var calcLinearMoment3 = new CalcLinearMoment(3, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a", "A");
            var areas = new List<CalcLinearMoment>() { calcLinearMoment1, calcLinearMoment2, calcLinearMoment3 };

            // Act
            CalcLinearMoment sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("kN·m/m", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(1, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a", "A");
            var calcLinearMoment2 = new CalcLinearMoment(2, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a", "A");
            var calcLinearMoment3 = new CalcLinearMoment(3, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a", "A");
            var areas = new List<CalcLinearMoment>() { calcLinearMoment1, calcLinearMoment2, calcLinearMoment3 };

            // Act
            CalcLinearMoment sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("kN·m/m", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(val1, MomentPerLengthUnit.NewtonMeterPerMeter, "q2", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(val2 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "q2", "Q");

            // Act
            bool result = calcLinearMoment1 == calcLinearMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(val1, MomentPerLengthUnit.NewtonMeterPerMeter, "q2", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(val2 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "q2", "Q");

            // Act
            bool result = calcLinearMoment1 != calcLinearMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(val1, MomentPerLengthUnit.NewtonMeterPerMeter, "q2", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(val2 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "q2", "Q");

            // Act
            bool result = calcLinearMoment1 > calcLinearMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(val1, MomentPerLengthUnit.NewtonMeterPerMeter, "q2", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(val2 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "q2", "Q");

            // Act
            bool result = calcLinearMoment1 < calcLinearMoment2;

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
            var calcLinearMoment1 = new CalcLinearMoment(val1, MomentPerLengthUnit.NewtonMeterPerMeter, "q2", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(val2 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "q2", "Q");

            // Act
            bool result = calcLinearMoment1 >= calcLinearMoment2;

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
            var calcLinearMoment1 = new CalcLinearMoment(val1, MomentPerLengthUnit.NewtonMeterPerMeter, "q2", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(val2 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "q2", "Q");

            // Act
            bool result = calcLinearMoment1 <= calcLinearMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearMoment.Equals((object)calcLinearMoment));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLinearMoment.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearMoment1.Equals((object)calcLinearMoment2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");
            var notCalcLinearMoment = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcLinearMoment.Equals(notCalcLinearMoment));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearMoment.Equals(calcLinearMoment));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLinearMoment.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearMoment1.Equals(calcLinearMoment2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcLinearMoment1 = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");
            var calcLinearMoment2 = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");
            var calcLinearMoment3 = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcLinearMoment1.GetHashCode() == calcLinearMoment2.GetHashCode();
            bool firstEqualsThird = calcLinearMoment1.GetHashCode() == calcLinearMoment3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "myQuantity", "Q");

            // Act
            string value = calcLinearMoment.GetValue();

            // Assert
            Assert.Equal("4.5 lbf·ft/ft", value);
        }
    }
}
