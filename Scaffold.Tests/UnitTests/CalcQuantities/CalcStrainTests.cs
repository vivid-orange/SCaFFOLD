using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcStrainTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.DecimalFraction, "ratio", "r");

            // Act
            // Assert
            Assert.True(CalcStrain.TryParse("5.5 %", null, out calcStrain));
            Assert.Equal(5.5, calcStrain.Value);
            Assert.Equal("%", calcStrain.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcStrain = CalcStrain.Parse("5.5 %", null);

            // Assert
            Assert.Equal(5.5, calcStrain.Value);
            Assert.Equal("%", calcStrain.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcQuantity = new CalcStrain(4.5, StrainUnit.DecimalFraction, "ratio", "r");

            // Act
            // Assert
            Assert.False(CalcStrain.TryParse("two hundred horses", null, out calcQuantity));
            Assert.Null(calcQuantity);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcStrain = new Strain(4.5, StrainUnit.DecimalFraction);

            // Act
            CalcStrain value = calcStrain;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.TypeName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.DecimalFraction, "ratio", "r");

            // Act
            double value = calcStrain;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.DecimalFraction, "ratio", "r");

            // Act
            Strain value = calcStrain;

            // Assert
            Assert.Equal(4.5, value.DecimalFractions);
            Assert.Equal(StrainUnit.DecimalFraction, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "r1", "R");
            var calcStrain2 = new CalcStrain(0.055, StrainUnit.DecimalFraction, "r2", "R");

            // Act
            CalcStrain result = calcStrain1 + calcStrain2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("R", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("r1 + r2", result.TypeName); // note: using Thin Space \u2009
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "a1", "A");

            // Act
            CalcStrain result = 2.0 + calcStrain;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcStrain.Value);
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "r1", "R");
            var calcStrain2 = new CalcStrain(0.055, StrainUnit.DecimalFraction, "r2", "R");

            // Act
            CalcStrain result = calcStrain1 - calcStrain2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("R", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("r1 - r2", result.TypeName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "a1", "A");

            // Act
            CalcStrain result = calcStrain - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcStrain.Value);
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "a1", "A");

            // Act
            CalcStrain result = -calcStrain;

            // Assert
            Assert.Equal(-4.5, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("-a1", result.TypeName);
        }


        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "a1", "A");

            // Act
            CalcStrain result = 2.0 * calcStrain;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcStrain.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "a1", "A");

            // Act
            CalcStrain result = calcStrain / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("a1", result.TypeName);
            Assert.Equal(4.5, calcStrain.Value);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "a1", "A");
            var calcStrain2 = new CalcStrain(5.5, StrainUnit.Percent, "a2", "A");

            // Act
            CalcDouble result = calcStrain1 / calcStrain2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("a1 / a2", result.TypeName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(1, StrainUnit.Percent, "a", "A");
            var calcStrain2 = new CalcStrain(2, StrainUnit.Percent, "a", "A");
            var calcStrain3 = new CalcStrain(3, StrainUnit.Percent, "a", "A");
            var areas = new List<CalcStrain>() { calcStrain1, calcStrain2, calcStrain3 };

            // Act
            CalcStrain sum = areas.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("%", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(1, StrainUnit.Percent, "a", "A");
            var calcStrain2 = new CalcStrain(2, StrainUnit.Percent, "a", "A");
            var calcStrain3 = new CalcStrain(3, StrainUnit.Percent, "a", "A");
            var areas = new List<CalcStrain>() { calcStrain1, calcStrain2, calcStrain3 };

            // Act
            CalcStrain sum = areas.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("%", sum.Unit);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStrain1 = new CalcStrain(val1, StrainUnit.DecimalFraction, "q2", "Q");
            var calcStrain2 = new CalcStrain(val2 * 100, StrainUnit.Percent, "q2", "Q");

            // Act
            bool result = calcStrain1 == calcStrain2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStrain1 = new CalcStrain(val1, StrainUnit.DecimalFraction, "q2", "Q");
            var calcStrain2 = new CalcStrain(val2 * 100, StrainUnit.Percent, "q2", "Q");

            // Act
            bool result = calcStrain1 != calcStrain2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStrain1 = new CalcStrain(val1, StrainUnit.DecimalFraction, "q2", "Q");
            var calcStrain2 = new CalcStrain(val2 * 100, StrainUnit.Percent, "q2", "Q");

            // Act
            bool result = calcStrain1 > calcStrain2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcStrain1 = new CalcStrain(val1, StrainUnit.DecimalFraction, "q2", "Q");
            var calcStrain2 = new CalcStrain(val2 * 100, StrainUnit.Percent, "q2", "Q");

            // Act
            bool result = calcStrain1 < calcStrain2;

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
            var calcStrain1 = new CalcStrain(val1, StrainUnit.DecimalFraction, "q2", "Q");
            var calcStrain2 = new CalcStrain(val2 * 100, StrainUnit.Percent, "q2", "Q");

            // Act
            bool result = calcStrain1 >= calcStrain2;

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
            var calcStrain1 = new CalcStrain(val1, StrainUnit.DecimalFraction, "q2", "Q");
            var calcStrain2 = new CalcStrain(val2 * 100, StrainUnit.Percent, "q2", "Q");

            // Act
            bool result = calcStrain1 <= calcStrain2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStrain.Equals((object)calcStrain));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcStrain.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");
            var calcStrain2 = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStrain1.Equals((object)calcStrain2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");
            var notCalcStrain = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcStrain.Equals(notCalcStrain));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStrain.Equals(calcStrain));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcStrain.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");
            var calcStrain2 = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcStrain1.Equals(calcStrain2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");
            var calcStrain2 = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");
            var calcStrain3 = new CalcStrain(4.5, StrainUnit.Percent, "MyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcStrain1.GetHashCode() == calcStrain2.GetHashCode();
            bool firstEqualsThird = calcStrain1.GetHashCode() == calcStrain3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Percent, "myQuantity", "Q");

            // Act
            string value = calcStrain.GetValueAsString();

            // Assert
            Assert.Equal("4.5 %", value);
        }
    }
}
