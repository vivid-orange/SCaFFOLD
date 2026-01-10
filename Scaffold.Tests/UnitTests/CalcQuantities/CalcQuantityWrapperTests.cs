using Scaffold.Core.CalcQuantities;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcQuantityWrapperTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH");

            // Act
            // Assert
            Assert.True(CalcQuantityWrapper<RelativeHumidity>.TryParse("90 %RH", null, out calcHumidity));
            Assert.Equal(90, calcHumidity.Value);
            Assert.Equal("%RH", calcHumidity.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcHumidity = CalcQuantityWrapper<RelativeHumidity>.Parse("90 %RH", null);

            // Assert
            Assert.Equal(90, calcHumidity.Value);
            Assert.Equal("%RH", calcHumidity.Unit);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcQuantity = new RelativeHumidity(4.5, RelativeHumidityUnit.Percent);

            // Act
            CalcQuantityWrapper<RelativeHumidity> value = calcQuantity;

            // Assert
            Assert.Equal(4.5, value.Value);
            Assert.Equal(string.Empty, value.TypeName);
            Assert.Equal(string.Empty, value.Symbol);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH");

            // Act
            // Assert
            Assert.False(CalcQuantityWrapper<RelativeHumidity>.TryParse("two hundred horses", null, out calcHumidity));
            Assert.Null(calcHumidity);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH");

            // Act
            double value = calcHumidity;

            // Assert
            Assert.Equal(70, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH");

            // Act
            RelativeHumidity value = calcHumidity;

            // Assert
            Assert.Equal(70, value.Percent);
            Assert.Equal(RelativeHumidityUnit.Percent, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(30, RelativeHumidityUnit.Percent), "Relative Humidity", "RH1");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH2");

            // Act
            CalcQuantityWrapper<RelativeHumidity> result = calcHumidity1 + calcHumidity2;

            // Assert
            Assert.Equal(100, result.Value);
            Assert.Equal("%RH", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.TypeName));
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(30, RelativeHumidityUnit.Percent), "Relative Humidity", "RH1");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH2");

            // Act
            CalcQuantityWrapper<RelativeHumidity> result = calcHumidity2 - calcHumidity1;

            // Assert
            Assert.Equal(40, result.Value);
            Assert.Equal("%RH", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.TypeName));
        }

        [Fact]
        public void UnaryNegationOperatorTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH");

            // Act
            CalcQuantityWrapper<RelativeHumidity> result = -calcHumidity;

            // Assert
            Assert.Equal(-70, result.Value);
            Assert.Equal("%RH", result.Unit);
            Assert.Equal("-Relative Humidity", result.TypeName);
            Assert.Equal("RH", result.Symbol);
        }


        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH");

            // Act
            CalcQuantityWrapper<RelativeHumidity> result = 2.0 + calcHumidity;

            // Assert
            Assert.Equal(72, result.Value);
            Assert.Equal("%RH", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.TypeName));
            Assert.Equal(70, calcHumidity.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(
                new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative Humidity", "RH");

            // Act
            CalcQuantityWrapper<RelativeHumidity> result = calcHumidity - 2.0;

            // Assert
            Assert.Equal(68, result.Value);
            Assert.Equal("%RH", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.TypeName));
            Assert.Equal(70, calcHumidity.Value);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val1, RelativeHumidityUnit.Percent), "q2", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val2, RelativeHumidityUnit.Percent), "q2", "Q");

            // Act
            bool result = calcHumidity1 == calcHumidity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val1, RelativeHumidityUnit.Percent), "q2", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val2, RelativeHumidityUnit.Percent), "q2", "Q");

            // Act
            bool result = calcHumidity1 != calcHumidity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val1, RelativeHumidityUnit.Percent), "q2", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val2, RelativeHumidityUnit.Percent), "q2", "Q");

            // Act
            bool result = calcHumidity1 > calcHumidity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val1, RelativeHumidityUnit.Percent), "q2", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val2, RelativeHumidityUnit.Percent), "q2", "Q");

            // Act
            bool result = calcHumidity1 < calcHumidity2;

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
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val1, RelativeHumidityUnit.Percent), "q2", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val2, RelativeHumidityUnit.Percent), "q2", "Q");

            // Act
            bool result = calcHumidity1 >= calcHumidity2;

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
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val1, RelativeHumidityUnit.Percent), "q2", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(val2, RelativeHumidityUnit.Percent), "q2", "Q");

            // Act
            bool result = calcHumidity1 <= calcHumidity2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcHumidity.Equals((object)calcHumidity));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcHumidity.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcHumidity1.Equals((object)calcHumidity2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");
            var notCalcQuantityWrapper = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcHumidity.Equals(notCalcQuantityWrapper));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcHumidity.Equals(calcHumidity));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");

            // Act
            // Assert
            Assert.False(calcHumidity.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");

            // Act
            // Assert
            Assert.True(calcHumidity1.Equals(calcHumidity2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcHumidity1 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");
            var calcHumidity2 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");
            var calcHumidity3 = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "notMyQuantity", "Q");

            // Act
            bool firstEqualsSecond = calcHumidity1.GetHashCode() == calcHumidity2.GetHashCode();
            bool firstEqualsThird = calcHumidity1.GetHashCode() == calcHumidity3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcHumidity = new CalcQuantityWrapper<RelativeHumidity>(new RelativeHumidity(4.5, RelativeHumidityUnit.Percent), "myQuantity", "Q");

            // Act
            string value = calcHumidity.GetValueAsString();

            // Assert
            Assert.Equal("4.5 %RH", value);
        }
    }
}
