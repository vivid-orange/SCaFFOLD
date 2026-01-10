using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcLinearForceTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "force", "w");

            // Act
            // Assert
            Assert.True(CalcLinearForce.TryParse("5.5 kN/m", null, out calcForce));
            Assert.Equal(5.5, calcForce.Value);
            Assert.Equal("kN/m", calcForce.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcForce = CalcLinearForce.Parse("5.5 kN/m", null);

            // Assert
            Assert.Equal(5.5, calcForce.Value);
            Assert.Equal("kN/m", calcForce.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "force", "w");

            // Act
            // Assert
            Assert.False(CalcLinearForce.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcLinearForce = new ForcePerLength(4.5, ForcePerLengthUnit.CentinewtonPerCentimeter);

            // Act
            CalcLinearForce value = calcLinearForce;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.TypeName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, "force", "w");

            // Act
            double value = calcForce;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, "force", "w");

            // Act
            ForcePerLength value = calcForce;

            // Assert
            Assert.Equal(4.5, value.KilonewtonsPerMeter);
            Assert.Equal(ForcePerLengthUnit.KilonewtonPerMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcLinearForce(4.5, ForcePerLengthUnit.NewtonPerMeter, "w1", "w");
            var calcForce2 = new CalcLinearForce(5.5 / 1000, ForcePerLengthUnit.KilonewtonPerMeter, "w2", "w");

            // Act
            CalcLinearForce result = calcForce1 + calcForce2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("w", result.Symbol);
            Assert.Equal("N/m", result.Unit);
            Assert.Equal("w1 + w2", result.TypeName);  // note: using Thin Space
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcLinearForce(4.5, ForcePerLengthUnit.NewtonPerMeter, "w1", "w");
            var calcForce2 = new CalcLinearForce(5.5 / 1000, ForcePerLengthUnit.KilonewtonPerMeter, "w2", "w");

            // Act
            CalcLinearForce result = calcForce1 - calcForce2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("w", result.Symbol);
            Assert.Equal("N/m", result.Unit);
            Assert.Equal("w1 - w2", result.TypeName);  // note: using Thin Space
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcForce = new CalcLinearForce(4.5, ForcePerLengthUnit.NewtonPerMeter, "w1", "w");

            // Act
            CalcLinearForce result = -calcForce;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("w", result.Symbol);
            Assert.Equal("N/m", result.Unit);
            Assert.Equal("-w1", result.TypeName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "w", "w");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcForce result = calcLength * calcForce; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN", result.Unit);
            Assert.Equal("w · l", result.TypeName);  // note: using Thin Space
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "w", "w");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcStress result = calcForce / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("w / l", result.TypeName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m²", result.Unit);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcLinearForce(4.5, "w1", "w");
            var calcForce2 = new CalcLinearForce(5.5, "w2", "w");

            // Act
            CalcDouble result = calcForce1 / calcForce2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("w1 / w2", result.TypeName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = 2.0 + calcLinearForce;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = calcLinearForce - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = 2.0 * calcLinearForce;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = calcLinearForce / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(1, ForcePerLengthUnit.KilonewtonPerMeter, "a", "A");
            var calcLinearForce2 = new CalcLinearForce(2, ForcePerLengthUnit.KilonewtonPerMeter, "a", "A");
            var calcLinearForce3 = new CalcLinearForce(3, ForcePerLengthUnit.KilonewtonPerMeter, "a", "A");
            var areas = new List<CalcLinearForce>() { calcLinearForce1, calcLinearForce2, calcLinearForce3 };

            // Act
            CalcLinearForce sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("kN/m", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(1, ForcePerLengthUnit.KilonewtonPerMeter, "a", "A");
            var calcLinearForce2 = new CalcLinearForce(2, ForcePerLengthUnit.KilonewtonPerMeter, "a", "A");
            var calcLinearForce3 = new CalcLinearForce(3, ForcePerLengthUnit.KilonewtonPerMeter, "a", "A");
            var areas = new List<CalcLinearForce>() { calcLinearForce1, calcLinearForce2, calcLinearForce3 };

            // Act
            CalcLinearForce sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("kN/m", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(val1, ForcePerLengthUnit.KilonewtonPerMeter, "q2", "Q");
            var calcLinearForce2 = new CalcLinearForce(val2 * 0.01, ForcePerLengthUnit.KilonewtonPerCentimeter, "q2", "Q");

            // Act
            bool result = calcLinearForce1 == calcLinearForce2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(val1, ForcePerLengthUnit.KilonewtonPerMeter, "q2", "Q");
            var calcLinearForce2 = new CalcLinearForce(val2 * 0.01, ForcePerLengthUnit.KilonewtonPerCentimeter, "q2", "Q");

            // Act
            bool result = calcLinearForce1 != calcLinearForce2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(val1, ForcePerLengthUnit.KilonewtonPerMeter, "q2", "Q");
            var calcLinearForce2 = new CalcLinearForce(val2 * 0.01, ForcePerLengthUnit.KilonewtonPerCentimeter, "q2", "Q");

            // Act
            bool result = calcLinearForce1 > calcLinearForce2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(val1, ForcePerLengthUnit.KilonewtonPerMeter, "q2", "Q");
            var calcLinearForce2 = new CalcLinearForce(val2 * 0.01, ForcePerLengthUnit.KilonewtonPerCentimeter, "q2", "Q");

            // Act
            bool result = calcLinearForce1 < calcLinearForce2;

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
            var calcLinearForce1 = new CalcLinearForce(val1, ForcePerLengthUnit.KilonewtonPerMeter, "q2", "Q");
            var calcLinearForce2 = new CalcLinearForce(val2 * 0.01, ForcePerLengthUnit.KilonewtonPerCentimeter, "q2", "Q");

            // Act
            bool result = calcLinearForce1 >= calcLinearForce2;

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
            var calcLinearForce1 = new CalcLinearForce(val1, ForcePerLengthUnit.KilonewtonPerMeter, "q2", "Q");
            var calcLinearForce2 = new CalcLinearForce(val2 * 0.01, ForcePerLengthUnit.KilonewtonPerCentimeter, "q2", "Q");

            // Act
            bool result = calcLinearForce1 <= calcLinearForce2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearForce.Equals((object)calcLinearForce));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLinearForce.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");
            var calcLinearForce2 = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearForce1.Equals((object)calcLinearForce2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");
            var notCalcLinearForce = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcLinearForce.Equals(notCalcLinearForce));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearForce.Equals(calcLinearForce));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcLinearForce.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");
            var calcLinearForce2 = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcLinearForce1.Equals(calcLinearForce2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcLinearForce1 = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");
            var calcLinearForce2 = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");
            var calcLinearForce3 = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcLinearForce1.GetHashCode() == calcLinearForce2.GetHashCode();
            bool firstEqualsThird = calcLinearForce1.GetHashCode() == calcLinearForce3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "myQuantity", "Q");

            // Act
            string value = calcLinearForce.GetValueAsString();

            // Assert
            Assert.Equal("4.5 lbf/ft", value);
        }
    }
}
