using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Exceptions;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcQuantityTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(calcArea.TryParse("5.5 cm²"));
            Assert.Equal(5.5, calcArea.Value);
            Assert.Equal("cm²", calcArea.Unit);
        }

        [Fact]
        public void TryParseFromStringWithWrongUnitTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.False(calcArea.TryParse("5.5 cm"));
        }

        [Fact]
        public void TryParseFromStringWithoutUnitTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(calcArea.TryParse("5.5"));
            Assert.Equal(5.5, calcArea.Value);
            Assert.Equal("ft²", calcArea.Unit);
        }

        [Fact]
        public void WrongUnitExceptionTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");
            Length length = new Length(6, LengthUnit.Centimeter);

            // Act
            // Assert
            Assert.Throws<UnitsNotSameException>(() => calcArea.Quantity = length);
        }
    }
}
