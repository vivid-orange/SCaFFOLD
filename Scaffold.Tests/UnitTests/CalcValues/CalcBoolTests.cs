using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcBoolTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcBool = new CalcBool(false);

            // Act
            // Assert
            Assert.True(calcBool.TryParse("true"));
            Assert.True(calcBool.Value);
        }

        [Fact]
        public void ImplicitFromWrappedTypeTest()
        {
            // Arrange
            var calcBool1 = new CalcBool(false);

            // Act
            calcBool1 = true;
            CalcBool calcBool2 = false;

            // Assert
            Assert.True(calcBool1.Value);
            Assert.False(calcBool2.Value);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, true)]
        public void EqualOperatorTest(bool val1, bool val2, bool expected)
        {
            // Arrange
            var calcBool1 = new CalcBool(val1);
            var calcBool2 = new CalcBool(val2);

            // Act
            bool result = calcBool1 == calcBool2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, true, true)]
        [InlineData(false, false, false)]
        public void NotEqualOperatorTest(bool val1, bool val2, bool expected)
        {
            // Arrange
            var calcBool1 = new CalcBool(val1);
            var calcBool2 = new CalcBool(val2);

            // Act
            bool result = calcBool1 != calcBool2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcBool = new CalcBool(false, "boolio", "B");

            // Act
            // Assert
            Assert.True(calcBool.Equals((object)calcBool));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcBool = new CalcBool(true, "boolio", "B");

            // Act
            // Assert
            Assert.False(calcBool.Equals((object)null));
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Arrange
            var calcBool1 = new CalcBool();

            // Act
            bool result = calcBool1;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AndOperatorTest()
        {
            // Arrange
            var calcBooltrue = new CalcBool(true, "truth");
            var calcBoolfalse = new CalcBool(false, "fake");

            // Act
            CalcBool expectedFalse = calcBooltrue & calcBoolfalse;
            CalcBool expectedTrue = calcBooltrue & calcBooltrue;
            CalcBool expectedFalse2 = calcBoolfalse & calcBoolfalse;

            // Assert
            Assert.False(expectedFalse);
            Assert.Equal("truth & fake", expectedFalse.DisplayName); // note: using Thin Space \u2009
            Assert.True(expectedTrue);
            Assert.Equal("truth & truth", expectedTrue.DisplayName); // note: using Thin Space \u2009
            Assert.False(expectedFalse2);
            Assert.Equal("fake & fake", expectedFalse2.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void OrOperatorTest()
        {
            // Arrange
            var calcBooltrue = new CalcBool(true, "truth");
            var calcBoolfalse = new CalcBool(false, "fake");

            // Act
            CalcBool expectedTrue = calcBooltrue | calcBoolfalse;
            CalcBool expectedTrue2 = calcBooltrue | calcBooltrue;
            CalcBool expectedFalse = calcBoolfalse | calcBoolfalse;

            // Assert
            Assert.True(expectedTrue);
            Assert.Equal("truth ∨ fake", expectedTrue.DisplayName); // note: using Thin Space \u2009
            Assert.True(expectedTrue2);
            Assert.Equal("truth ∨ truth", expectedTrue2.DisplayName); // note: using Thin Space \u2009
            Assert.False(expectedFalse);
            Assert.Equal("fake ∨ fake", expectedFalse.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void EqualsOperatorTest()
        {
            // Arrange
            var calcBooltrue1 = new CalcBool(true, "truth1");
            var calcBooltrue2 = new CalcBool(true, "truth2");
            var calcBoolfalse1 = new CalcBool(false, "fake1");
            var calcBoolfalse2 = new CalcBool(false, "fake2");

            // Act
            CalcBool expectedFalse1 = calcBooltrue1 == calcBoolfalse1;
            CalcBool expectedTrue = calcBooltrue1 == calcBooltrue2;
            CalcBool expectedTrue2 = calcBoolfalse1 == calcBoolfalse2;

            // Assert
            Assert.False(expectedFalse1);
            Assert.Equal("truth1 = fake1", expectedFalse1.DisplayName); // note: using Thin Space \u2009
            Assert.True(expectedTrue);
            Assert.Equal("truth1 = truth2", expectedTrue.DisplayName); // note: using Thin Space \u2009
            Assert.True(expectedTrue2);
            Assert.Equal("fake1 = fake2", expectedTrue2.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void NotEqualsOperatorTest()
        {
            // Arrange
            var calcBooltrue1 = new CalcBool(true, "truth1");
            var calcBooltrue2 = new CalcBool(true, "truth2");
            var calcBoolfalse1 = new CalcBool(false, "fake1");
            var calcBoolfalse2 = new CalcBool(false, "fake2");

            // Act
            CalcBool expectedTrue1 = calcBooltrue1 != calcBoolfalse1;
            CalcBool expectedFalse1 = calcBooltrue1 != calcBooltrue2;
            CalcBool expectedFalse2 = calcBoolfalse1 != calcBoolfalse2;

            // Assert
            Assert.True(expectedTrue1);
            Assert.Equal("truth1 ≠ fake1", expectedTrue1.DisplayName); // note: using Thin Space \u2009
            Assert.False(expectedFalse1);
            Assert.Equal("truth1 ≠ truth2", expectedFalse1.DisplayName); // note: using Thin Space \u2009
            Assert.False(expectedFalse2);
            Assert.Equal("fake1 ≠ fake2", expectedFalse2.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void EqualsTest()
        {
            // Arrange
            var calcBooltrue = new CalcBool(true, "truth");
            var calcBoolfalse = new CalcBool(false, "fake");

            // Act
            // Assert
            Assert.False(calcBooltrue.Equals(calcBoolfalse));
            Assert.True(calcBooltrue.Equals(calcBooltrue));
            Assert.True(calcBoolfalse.Equals(calcBoolfalse));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcBool1 = new CalcBool(true, "some", "S");
            var calcBool2 = new CalcBool(true, "some", "S");
            var calcBool3 = new CalcBool(false, "some", "S");

            // Act
            bool firstEqualsSecond = calcBool1.GetHashCode() == calcBool2.GetHashCode();
            bool firstEqualsThird = calcBool1.GetHashCode() == calcBool3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }
    }
}
