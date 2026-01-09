using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcForceTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, ForceUnit.PoundForce, "force", "F");

            // Act
            // Assert
            Assert.True(CalcForce.TryParse("5.5 kN", null, out calcForce));
            Assert.Equal(5.5, calcForce.Value);
            Assert.Equal("kN", calcForce.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcForce = CalcForce.Parse("5.5 kN", null);

            // Assert
            Assert.Equal(5.5, calcForce.Value);
            Assert.Equal("kN", calcForce.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(CalcForce.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcForce = new Force(4.5, ForceUnit.Kilonewton);

            // Act
            CalcForce value = calcForce;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.DisplayName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, "force", "F");

            // Act
            double value = calcForce;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, "force", "F");

            // Act
            Force value = calcForce;

            // Assert
            Assert.Equal(4.5, value.Kilonewtons);
            Assert.Equal(ForceUnit.Kilonewton, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcForce(4.5, ForceUnit.Newton, "f1", "F");
            var calcForce2 = new CalcForce(5.5 / 1000, ForceUnit.Kilonewton, "f2", "F");

            // Act
            CalcForce result = calcForce1 + calcForce2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("f1 + f2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcForce(4.5, ForceUnit.Newton, "f1", "F");
            var calcForce2 = new CalcForce(5.5 / 1000, ForceUnit.Kilonewton, "f2", "F");

            // Act
            CalcForce result = calcForce1 - calcForce2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("f1 - f2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "f1", "F");

            // Act
            CalcForce result = -calcForce;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("-f1", result.DisplayName);
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, "f", "F");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcMoment result = calcLength * calcForce;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("f · l", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcForce(4.5, "f1", "F");
            var calcForce2 = new CalcForce(5.5, "f2", "F");

            // Act
            CalcDouble result = calcForce1 / calcForce2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("f1 / f2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, ForceUnit.Kilonewton, "f", "F");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcLinearForce result = calcForce / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("f / l", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m", result.Unit);
        }

        [Fact]
        public void DivisionAreaOperatorTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, ForceUnit.Kilonewton, "f", "F");
            var calcArea = new CalcArea(5.5, AreaUnit.SquareMeter, "A", "A");

            // Act
            CalcStress result = calcForce / calcArea;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("f / A", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m²", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "myForce", "F");

            // Act
            CalcForce result = 2.0 + calcForce;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("myForce", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "myForce", "F");

            // Act
            CalcForce result = calcForce - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("myForce", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "myForce", "F");

            // Act
            CalcForce result = 2.0 * calcForce;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("myForce", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "myForce", "F");

            // Act
            CalcForce result = calcForce / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("myForce", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcForce1 = new CalcForce(1, ForceUnit.Kilonewton, "f1", "F");
            var calcForce2 = new CalcForce(2, ForceUnit.Kilonewton, "f2", "F");
            var calcForce3 = new CalcForce(3, ForceUnit.Kilonewton, "f3", "F");
            var forces = new List<CalcForce>() { calcForce1, calcForce2, calcForce3 };

            // Act
            CalcForce sum = forces.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("kN", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcForce1 = new CalcForce(1, ForceUnit.Kilonewton, "f1", "F");
            var calcForce2 = new CalcForce(2, ForceUnit.Kilonewton, "f2", "F");
            var calcForce3 = new CalcForce(3, ForceUnit.Kilonewton, "f3", "F");
            var forces = new List<CalcForce>() { calcForce1, calcForce2, calcForce3 };

            // Act
            CalcForce sum = forces.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("kN", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcQuantity1 = new CalcForce(val1, ForceUnit.Kilonewton, "q1", "Q");
            var calcQuantity2 = new CalcForce(val2 * 1000, ForceUnit.Newton, "q2", "Q");

            // Act
            bool result = calcQuantity1 == calcQuantity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcQuantity1 = new CalcForce(val1, ForceUnit.Kilonewton, "q1", "Q");
            var calcQuantity2 = new CalcForce(val2 * 1000, ForceUnit.Newton, "q2", "Q");

            // Act
            bool result = calcQuantity1 != calcQuantity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcQuantity1 = new CalcForce(val1, ForceUnit.Kilonewton, "q1", "Q");
            var calcQuantity2 = new CalcForce(val2 * 1000, ForceUnit.Newton, "q2", "Q");

            // Act
            bool result = calcQuantity1 > calcQuantity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcQuantity1 = new CalcForce(val1, ForceUnit.Kilonewton, "q1", "Q");
            var calcQuantity2 = new CalcForce(val2 * 1000, ForceUnit.Newton, "q2", "Q");

            // Act
            bool result = calcQuantity1 < calcQuantity2;

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
            var calcQuantity1 = new CalcForce(val1, ForceUnit.Kilonewton, "q1", "Q");
            var calcQuantity2 = new CalcForce(val2 * 1000, ForceUnit.Newton, "q2", "Q");

            // Act
            bool result = calcQuantity1 >= calcQuantity2;

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
            var calcQuantity1 = new CalcForce(val1, ForceUnit.Kilonewton, "q1", "Q");
            var calcQuantity2 = new CalcForce(val2 * 1000, ForceUnit.Newton, "q2", "Q");

            // Act
            bool result = calcQuantity1 <= calcQuantity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcQuantity = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcQuantity.Equals((object)calcQuantity));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcQuantity = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcQuantity.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcQuantity1 = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");
            var calcQuantity2 = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcQuantity1.Equals((object)calcQuantity2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcQuantity = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");
            var notCalcForce = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcQuantity.Equals(notCalcForce));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcQuantity = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcQuantity.Equals(calcQuantity));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcQuantity = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcQuantity.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcQuantity1 = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");
            var calcQuantity2 = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcQuantity1.Equals(calcQuantity2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcQuantity1 = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");
            var calcQuantity2 = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");
            var calcQuantity3 = new CalcForce(4.5, ForceUnit.PoundForce, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcQuantity1.GetHashCode() == calcQuantity2.GetHashCode();
            bool firstEqualsThird = calcQuantity1.GetHashCode() == calcQuantity3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcQuantity = new CalcForce(4.5, ForceUnit.PoundForce, "myQuantity", "Q");

            // Act
            string value = calcQuantity.GetValue();

            // Assert
            Assert.Equal("4.5 lbf", value); // note: using Thin Space \u2009
        }
    }
}
