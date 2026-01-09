using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcMomentTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "moment", "M");

            // Act
            // Assert
            Assert.True(CalcMoment.TryParse("5.5 kN·m", null, out calcMoment));
            Assert.Equal(5.5, calcMoment.Value);
            Assert.Equal("kN·m", calcMoment.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcMoment = CalcMoment.Parse("5.5 kN·m", null);

            // Assert
            Assert.Equal(5.5, calcMoment.Value);
            Assert.Equal("kN·m", calcMoment.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcMoment(4.5, MomentUnit.PoundalFoot, "moment", "M");

            // Act
            // Assert
            Assert.False(CalcMoment.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcMoment = new Moment(4.5, MomentUnit.KilonewtonMeter);

            // Act
            CalcMoment value = calcMoment;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "moment", "M");

            // Act
            double value = calcMoment;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "moment", "M");

            // Act
            Moment value = calcMoment;

            // Assert
            Assert.Equal(4.5, value.KilonewtonMeters);
            Assert.Equal(MomentUnit.KilonewtonMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, MomentUnit.NewtonMeter, "m1", "M");
            var calcMoment2 = new CalcMoment(5.5 / 1000, MomentUnit.KilonewtonMeter, "m2", "M");

            // Act
            CalcMoment result = calcMoment1 + calcMoment2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m", result.Unit);
            Assert.Equal("m1 + m2", result.DisplayName);  // note: using Thin Space
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, MomentUnit.NewtonMeter, "m1", "M");
            var calcMoment2 = new CalcMoment(5.5 / 1000, MomentUnit.KilonewtonMeter, "m2", "M");

            // Act
            CalcMoment result = calcMoment1 - calcMoment2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m", result.Unit);
            Assert.Equal("m1 - m2", result.DisplayName);  // note: using Thin Space
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.NewtonMeter, "m1", "M");

            // Act
            CalcMoment result = -calcMoment;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m", result.Unit);
            Assert.Equal("-m1", result.DisplayName);
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "m", "M");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcForce result = calcMoment / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m / l", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN", result.Unit);
        }

        [Fact]
        public void DivisionForceOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "m", "M");
            var calcForce = new CalcForce(5.5, ForceUnit.Kilonewton, "f", "F");

            // Act
            CalcLength result = calcMoment / calcForce;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m / f", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("m", result.Unit);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, "m1", "M");
            var calcMoment2 = new CalcMoment(5.5, "m2", "M");

            // Act
            CalcDouble result = calcMoment1 / calcMoment2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m1 / m2", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void MakeLinearTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "m", "M");

            // Act
            CalcLinearMoment result = calcMoment.MakeLinear();

            // Assert
            Assert.Equal(4.5, result.Value, 12);
            Assert.Equal("m", result.DisplayName);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
        }

        [Fact]
        public void MakeLinearFromLengthTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.NewtonCentimeter, "m", "M");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcLinearMoment result = calcMoment.MakeLinear(calcLength);

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m", result.DisplayName);
            Assert.Equal("N·cm/m", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.KilonewtonMeter, "a1", "A");

            // Act
            CalcMoment result = 2.0 + calcMoment;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcMoment.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.KilonewtonMeter, "a1", "A");

            // Act
            CalcMoment result = calcMoment - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcMoment.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.KilonewtonMeter, "a1", "A");

            // Act
            CalcMoment result = 2.0 * calcMoment;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcMoment.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.KilonewtonMeter, "a1", "A");

            // Act
            CalcMoment result = calcMoment / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcMoment.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(1, MomentUnit.KilonewtonCentimeter, "a", "A");
            var calcMoment2 = new CalcMoment(2, MomentUnit.KilonewtonCentimeter, "a", "A");
            var calcMoment3 = new CalcMoment(3, MomentUnit.KilonewtonCentimeter, "a", "A");
            var areas = new List<CalcMoment>() { calcMoment1, calcMoment2, calcMoment3 };

            // Act
            CalcMoment sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("kN·cm", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(1, MomentUnit.KilonewtonCentimeter, "a", "A");
            var calcMoment2 = new CalcMoment(2, MomentUnit.KilonewtonCentimeter, "a", "A");
            var calcMoment3 = new CalcMoment(3, MomentUnit.KilonewtonCentimeter, "a", "A");
            var areas = new List<CalcMoment>() { calcMoment1, calcMoment2, calcMoment3 };

            // Act
            CalcMoment sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("kN·cm", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcMoment1 = new CalcMoment(val1, MomentUnit.KilonewtonMeter, "q2", "Q");
            var calcMoment2 = new CalcMoment(val2 * 100, MomentUnit.KilonewtonCentimeter, "q2", "Q");

            // Act
            bool result = calcMoment1 == calcMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcMoment1 = new CalcMoment(val1, MomentUnit.KilonewtonMeter, "q2", "Q");
            var calcMoment2 = new CalcMoment(val2 * 100, MomentUnit.KilonewtonCentimeter, "q2", "Q");

            // Act
            bool result = calcMoment1 != calcMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcMoment1 = new CalcMoment(val1, MomentUnit.KilonewtonMeter, "q2", "Q");
            var calcMoment2 = new CalcMoment(val2 * 100, MomentUnit.KilonewtonCentimeter, "q2", "Q");

            // Act
            bool result = calcMoment1 > calcMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcMoment1 = new CalcMoment(val1, MomentUnit.KilonewtonMeter, "q2", "Q");
            var calcMoment2 = new CalcMoment(val2 * 100, MomentUnit.KilonewtonCentimeter, "q2", "Q");

            // Act
            bool result = calcMoment1 < calcMoment2;

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
            var calcMoment1 = new CalcMoment(val1, MomentUnit.KilonewtonMeter, "q2", "Q");
            var calcMoment2 = new CalcMoment(val2 * 100, MomentUnit.KilonewtonCentimeter, "q2", "Q");

            // Act
            bool result = calcMoment1 >= calcMoment2;

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
            var calcMoment1 = new CalcMoment(val1, MomentUnit.KilonewtonMeter, "q2", "Q");
            var calcMoment2 = new CalcMoment(val2 * 100, MomentUnit.KilonewtonCentimeter, "q2", "Q");

            // Act
            bool result = calcMoment1 <= calcMoment2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcMoment.Equals((object)calcMoment));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcMoment.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");
            var calcMoment2 = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcMoment1.Equals((object)calcMoment2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");
            var notCalcMoment = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcMoment.Equals(notCalcMoment));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcMoment.Equals(calcMoment));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcMoment.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");
            var calcMoment2 = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcMoment1.Equals(calcMoment2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");
            var calcMoment2 = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");
            var calcMoment3 = new CalcMoment(4.5, MomentUnit.PoundalFoot, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcMoment1.GetHashCode() == calcMoment2.GetHashCode();
            bool firstEqualsThird = calcMoment1.GetHashCode() == calcMoment3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "myQuantity", "Q");

            // Act
            string value = calcMoment.GetValue();

            // Assert
            Assert.Equal("4.5 pdl·ft", value);
        }
    }
}
