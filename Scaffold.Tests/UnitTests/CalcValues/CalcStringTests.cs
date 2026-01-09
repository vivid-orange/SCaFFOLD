using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcStringTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcString = new CalcString("hello");

            // Act
            // Assert
            Assert.True(calcString.TryParse("olleh"));
            Assert.Equal("olleh", calcString.Value);
        }

        [Fact]
        public void ImplicitFromWrappedTypeTest()
        {
            // Arrange
            CalcString calcString1 = new CalcString("hello");

            // Act
            calcString1 = "bye bye";
            CalcString calcString2 = "soupdragon";

            // Assert
            Assert.Equal("bye bye", calcString1.Value);
            Assert.Equal("soupdragon", calcString2.Value);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            CalcString calcString1 = new CalcString("hello");

            // Act
            string result = calcString1;

            // Assert
            Assert.Equal("hello", result);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcString1 = new CalcString("hello");
            var calcString2 = new CalcString("friend");

            // Act
            calcString1 += calcString2;

            // Assert
            Assert.Equal("hellofriend", calcString1.Value);
        }

        [Fact]
        public void AdditionSameUnitOperatorTest()
        {
            // Arrange
            var calcString1 = new CalcString("hello", "l1", "L", "m");
            var calcString2 = new CalcString("friend", "l2", "L", "m");

            // Act
            CalcString result = calcString1 + calcString2;

            // Assert
            Assert.Equal("hellofriend", result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void Constructor_WithValue_SetsProperties()
        {
            // Arrange & Act
            var calcString = new CalcString("test value");

            // Assert
            Assert.Equal("test value", calcString.Value);
            Assert.True(string.IsNullOrEmpty(calcString.DisplayName));
            Assert.True(string.IsNullOrEmpty(calcString.Symbol));
            Assert.True(string.IsNullOrEmpty(calcString.Unit));
        }

        [Fact]
        public void Constructor_WithValueAndDisplayName_SetsProperties()
        {
            // Arrange & Act
            var calcString = new CalcString("test value", "display name");

            // Assert
            Assert.Equal("test value", calcString.Value);
            Assert.Equal("display name", calcString.DisplayName);
            Assert.True(string.IsNullOrEmpty(calcString.Symbol));
            Assert.True(string.IsNullOrEmpty(calcString.Unit));
        }

        [Fact]
        public void Constructor_WithValueDisplayNameAndSymbol_SetsProperties()
        {
            // Arrange & Act
            var calcString = new CalcString("test value", "display name", "α");

            // Assert
            Assert.Equal("test value", calcString.Value);
            Assert.Equal("display name", calcString.DisplayName);
            Assert.Equal("α", calcString.Symbol);
            Assert.True(string.IsNullOrEmpty(calcString.Unit));
        }

        [Fact]
        public void Constructor_WithValueDisplayNameSymbolAndUnit_SetsProperties()
        {
            // Arrange & Act
            var calcString = new CalcString("test value", "display name", "α", "unit");

            // Assert
            Assert.Equal("test value", calcString.Value);
            Assert.Equal("display name", calcString.DisplayName);
            Assert.Equal("α", calcString.Symbol);
            Assert.Equal("unit", calcString.Unit);
        }

        [Fact]
        public void ImplicitOperator_ToString_ReturnsValue()
        {
            // Arrange
            var calcString = new CalcString("test value");

            // Act
            string result = calcString;

            // Assert
            Assert.Equal("test value", result);
        }
    }
}
