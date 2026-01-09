using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcValueTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5);

            // Act
            // Assert
            Assert.False(calcDouble.TryParse("invalid"));
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 == calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 != calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5, "number", "N");

            // Act
            // Assert
            Assert.True(calcDouble.Equals((object)calcDouble));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5, "number", "N");

            // Act
            // Assert
            Assert.False(calcDouble.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5, "number", "N");
            var calcDouble2 = new CalcDouble(4.5, "number", "N");

            // Act
            // Assert
            Assert.True(calcDouble1.Equals((object)calcDouble2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5, "number", "N");
            var notCalcDouble = new CalcInt(4, "length", "l");

            // Act
            // Assert
            Assert.False(calcDouble.Equals(notCalcDouble));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5,
                "number", "N");

            // Act
            // Assert
            Assert.True(calcDouble.Equals(calcDouble));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5, "number", "N");

            // Act
            // Assert
            Assert.False(calcDouble.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5, "number", "N");
            var calcDouble2 = new CalcDouble(4.5, "number", "N");

            // Act
            // Assert
            Assert.True(calcDouble1.Equals(calcDouble2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5, "some", "S");
            var calcDouble2 = new CalcDouble(4.5, "some", "S");
            var calcDouble3 = new CalcDouble(4.5, "some", "s");

            // Act
            bool firstEqualsSecond = calcDouble1.GetHashCode() == calcDouble2.GetHashCode();
            bool firstEqualsThird = calcDouble1.GetHashCode() == calcDouble3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5, "engineers", "E", "Eng");

            // Act
            string value = calcDouble.GetValueAsString();

            // Assert
            Assert.Equal("4.5 Eng", value);
        }
    }
}
