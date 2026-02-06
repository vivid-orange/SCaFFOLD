using System.Globalization;
using FluentAssertions;
using Scaffold.Reader;
using UnitsNet;

namespace Scaffold.Tests.Reader;

public class DelegateCalcValueTests
{
    #region Helpers

    /// <summary>
    /// Creates an ICalcValue by using CalculationReader on a wrapper class.
    /// This tests the real integration path without accessing internal types.
    /// </summary>
    private static ICalcValue CreateCalcValue<T>(T initial)
    {
        var wrapper = new ValueWrapper<T> { Value = initial };
        var inputs = CalculationReader.GetInputs(wrapper);
        return inputs.First();
    }

    private static ICalcValue CreateReadOnlyCalcValue<T>(T initial)
    {
        var wrapper = new ReadOnlyWrapper<T>(initial);
        var outputs = CalculationReader.GetOutputs(wrapper);
        return outputs.First();
    }

    private static ICalcValue CreateCalcValueWithHeadings<T>(T initial, string[] headings)
    {
        var wrapper = new ValueWrapperWithHeadings<T> { Value = initial };
        var inputs = CalculationReader.GetInputs(wrapper);
        return inputs.First();
    }

    // Wrapper classes for creating ICalcValue via CalculationReader
    private class ValueWrapper<T>
    {
        [InputParameter("x", "Test")]
        public T Value { get; set; } = default!;
    }

    private class ReadOnlyWrapper<T>
    {
        public ReadOnlyWrapper(T value) => Value = value;

        [OutputParameter("x", "Test")]
        public T Value { get; }
    }

    private class ValueWrapperWithHeadings<T>
    {
        [InputParameter("x", "Test", ["Group1", "Group2"])]
        public T Value { get; set; } = default!;
    }

    #endregion

    #region IFormattable / ToString

    [Fact]
    public void ToString_DelegatesToUnderlyingIFormattable()
    {
        var length = new Length(3.14159, LengthUnit.Meter);
        ICalcValue cv = CreateCalcValue(length);

        string result = cv.ToString("F2", CultureInfo.InvariantCulture);

        result.Should().Be(length.ToString("F2", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ToString_Collection_ReturnsCountSummary()
    {
        var list = new List<int> { 1, 2, 3 };
        ICalcValue cv = CreateCalcValue(list);

        string result = cv.ToString(null, CultureInfo.InvariantCulture);

        result.Should().Be("List`1 (3 items)");
    }

    [Fact]
    public void ToString_Override_UsesInvariantCulture()
    {
        var length = new Length(1.5, LengthUnit.Meter);
        ICalcValue cv = CreateCalcValue(length);

        string overrideResult = cv.ToString();
        string formattableResult = cv.ToString(null, CultureInfo.InvariantCulture);

        overrideResult.Should().Be(formattableResult);
    }

    [Fact]
    public void ToString_NullValue_ReturnsEmpty()
    {
        string? val = null;
        ICalcValue cv = CreateCalcValue(val);

        cv.ToString().Should().Be(string.Empty);
    }

    #endregion

    #region TryParse — IQuantity

    [Fact]
    public void TryParse_IQuantity_FullUnitString_Succeeds()
    {
        ICalcValue cv = CreateCalcValue(new Length(0, LengthUnit.Meter));

        bool result = cv.TryParse("5 m");

        result.Should().BeTrue();
        cv.ValueAsObject.Should().Be(new Length(5, LengthUnit.Meter));
    }

    [Fact]
    public void TryParse_IQuantity_BareNumber_PreservesUnit()
    {
        ICalcValue cv = CreateCalcValue(new Length(1, LengthUnit.Millimeter));

        bool result = cv.TryParse("10");

        result.Should().BeTrue();
        cv.ValueAsObject.Should().Be(new Length(10, LengthUnit.Millimeter));
    }

    [Fact]
    public void TryParse_IQuantity_InvalidString_ReturnsFalse()
    {
        var original = new Length(5, LengthUnit.Meter);
        ICalcValue cv = CreateCalcValue(original);

        bool result = cv.TryParse("not a number");

        result.Should().BeFalse();
        cv.ValueAsObject.Should().Be(original);
    }

    #endregion

    #region TryParse — IParsable / TypeConverter

    [Fact]
    public void TryParse_Int_Succeeds()
    {
        ICalcValue cv = CreateCalcValue(0);

        bool result = cv.TryParse("42");

        result.Should().BeTrue();
        cv.ValueAsObject.Should().Be(42);
    }

    [Fact]
    public void TryParse_Double_Succeeds()
    {
        ICalcValue cv = CreateCalcValue(0.0);

        bool result = cv.TryParse("3.14");

        result.Should().BeTrue();
        ((double)cv.ValueAsObject).Should().BeApproximately(3.14, 0.001);
    }

    [Fact]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        ICalcValue cv = CreateCalcValue(0);

        bool result = cv.TryParse("abc");

        result.Should().BeFalse();
        cv.ValueAsObject.Should().Be(0);
    }

    #endregion

    #region TryParse — read-only (output) values

    [Fact]
    public void TryParse_ReadOnly_ReturnsFalse()
    {
        ICalcValue cv = CreateReadOnlyCalcValue(42);

        bool result = cv.TryParse("99");

        result.Should().BeFalse();
    }

    #endregion

    #region TryParse — IQuantity Exception Handling

    [Fact]
    public void TryParse_IQuantity_UnitsNetException_TriesBareNumber()
    {
        ICalcValue cv = CreateCalcValue(new Length(5, LengthUnit.Meter));

        bool result = cv.TryParse("invalid unit");

        result.Should().BeFalse();
        cv.ValueAsObject.Should().Be(new Length(5, LengthUnit.Meter));
    }

    #endregion

    #region TryParse — TypeConverter Edge Cases

    [Fact]
    public void TryParse_TypeConverterThrows_ReturnsFalse()
    {
        ICalcValue cv = CreateCalcValue(new Guid("12345678-1234-1234-1234-123456789012"));

        // Try to parse with malformed GUID format
        bool result = cv.TryParse("not-a-guid");

        result.Should().BeFalse();
    }

    [Fact]
    public void TryParse_NoConverterAvailable_ReturnsFalse()
    {
        // Create a value type with no TypeConverter
        ICalcValue cv = CreateCalcValue(new object());

        bool result = cv.TryParse("anything");

        result.Should().BeFalse();
    }

    #endregion

    #region ToString — Edge Cases

    [Fact]
    public void ToString_StringType_DoesNotShowAsCollection()
    {
        ICalcValue cv = CreateCalcValue("hello world");

        string result = cv.ToString();

        result.Should().Be("hello world");
    }

    [Fact]
    public void ToString_EmptyCollection_ReturnsZeroItems()
    {
        var list = new List<int>();
        ICalcValue cv = CreateCalcValue(list);

        string result = cv.ToString();

        result.Should().Be("List`1 (0 items)");
    }

    [Fact]
    public void ToString_NonFormattable_UsesDefaultToString()
    {
        var obj = new object();
        ICalcValue cv = CreateCalcValue(obj);

        string result = cv.ToString();

        result.Should().Be(obj.ToString());
    }

    [Fact]
    public void ToString_WithFormat_PassesToIFormattable()
    {
        var length = new Length(1.23456, LengthUnit.Meter);
        ICalcValue cv = CreateCalcValue(length);

        string result = cv.ToString("F1", CultureInfo.InvariantCulture);

        result.Should().Contain("1.2");
    }

    #endregion

    #region ValueAsObject Property

    [Fact]
    public void ValueAsObject_ReturnsCurrentValue()
    {
        ICalcValue cv = CreateCalcValue(42);

        cv.ValueAsObject.Should().Be(42);
    }

    [Fact]
    public void ValueAsObject_AfterTryParse_ReturnsNewValue()
    {
        ICalcValue cv = CreateCalcValue(0);

        cv.TryParse("42");

        cv.ValueAsObject.Should().Be(42);
    }

    #endregion

    #region Headings

    [Fact]
    public void Headings_WhenDefined_ReturnsHeadings()
    {
        ICalcValue cv = CreateCalcValueWithHeadings(42, ["Group1", "Group2"]);

        cv.Headings.Should().HaveCount(2);
        cv.Headings.Should().ContainInOrder("Group1", "Group2");
    }

    #endregion

    #region TryParse — IParsable Path

    [Fact]
    public void TryParse_IParsableType_Succeeds()
    {
        ICalcValue cv = CreateCalcValue(Guid.Empty);

        bool result = cv.TryParse("12345678-1234-1234-1234-123456789abc");

        result.Should().BeTrue();
        cv.ValueAsObject.Should().Be(new Guid("12345678-1234-1234-1234-123456789abc"));
    }

    [Fact]
    public void TryParse_IParsableType_InvalidFormat_ReturnsFalse()
    {
        ICalcValue cv = CreateCalcValue(Guid.Empty);

        bool result = cv.TryParse("not-a-guid-format");

        result.Should().BeFalse();
        cv.ValueAsObject.Should().Be(Guid.Empty);
    }

    #endregion

    #region Type Flags

    [Theory]
    [InlineData(42)]
    [InlineData("hello")]
    [InlineData(3.14)]
    public void TypeFlags_Primitive_AllFalse(object primitive)
    {
        // Arrange - need to create via specific type
        ICalcValue cv = primitive switch
        {
            int i => CreateCalcValue(i),
            string s => CreateCalcValue(s),
            double d => CreateCalcValue(d),
            _ => throw new ArgumentException()
        };

        // Assert
        Assert.False(cv.IsICalculation);
        Assert.False(cv.IsCollection);
        Assert.False(cv.IsComplexValue);
    }

    [Fact]
    public void TypeFlags_ListOfInt_IsCollectionTrue()
    {
        // Arrange
        ICalcValue cv = CreateCalcValue(new List<int> { 1, 2 });

        // Assert
        Assert.False(cv.IsICalculation);
        Assert.True(cv.IsCollection);
    }

    [Fact]
    public void TypeFlags_Calculation_IsICalculationTrue()
    {
        // Arrange
        var calc = new StubCalculation();
        ICalcValue cv = CreateCalcValue<ICalculation>(calc);

        // Assert
        Assert.True(cv.IsICalculation);
    }

    #endregion

    #region CheckIfComplex

    [Fact]
    public void CheckIfComplex_TypeWithCalcParameterAttribute_IsComplexTrue()
    {
        // Arrange — ComplexStub has [InputParameter] on a property
        ICalcValue cv = CreateCalcValue(new ComplexStub());

        // Assert
        Assert.True(cv.IsComplexValue);
    }

    [Fact]
    public void CheckIfComplex_PlainClassNoAttributes_IsComplexFalse()
    {
        // Arrange
        ICalcValue cv = CreateCalcValue(new PlainStub());

        // Assert
        Assert.False(cv.IsComplexValue);
    }

    #endregion

    #region GetChildInputs / GetChildOutputs

    [Fact]
    public void GetChildInputs_NullValue_ReturnsEmptyList()
    {
        // Arrange
        string? val = null;
        ICalcValue cv = CreateCalcValue(val);

        // Act
        List<ICalcValue> children = cv.GetChildInputs();

        // Assert
        Assert.Empty(children);
    }

    [Fact]
    public void GetChildOutputs_NullValue_ReturnsEmptyList()
    {
        // Arrange
        string? val = null;
        ICalcValue cv = CreateCalcValue(val);

        // Act
        List<ICalcValue> children = cv.GetChildOutputs();

        // Assert
        Assert.Empty(children);
    }

    [Fact]
    public void GetChildInputs_ComplexObject_ReturnsInputs()
    {
        // Arrange
        var obj = new ComplexStub();
        ICalcValue cv = CreateCalcValue(obj);

        // Act
        List<ICalcValue> children = cv.GetChildInputs();

        // Assert
        Assert.NotEmpty(children);
        Assert.Contains(children, c => c.Symbol == "W");
    }

    [Fact]
    public void GetChildOutputs_ComplexObject_ReturnsOutputs()
    {
        // Arrange
        var obj = new ComplexStub();
        ICalcValue cv = CreateCalcValue(obj);

        // Act
        List<ICalcValue> children = cv.GetChildOutputs();

        // Assert
        Assert.Single(children);
        Assert.Equal("R", children[0].Symbol);
    }

    #endregion

    #region TryParse — IQuantity with different unit

    [Fact]
    public void TryParse_IQuantity_DifferentUnit_ParsesSuccessfully()
    {
        // Arrange — start in meters, parse millimeters
        ICalcValue cv = CreateCalcValue(new Length(1, LengthUnit.Meter));

        // Act
        bool result = cv.TryParse("5 mm");

        // Assert
        Assert.True(result);
        var length = (Length)cv.ValueAsObject;
        Assert.Equal(LengthUnit.Millimeter, length.Unit);
        Assert.Equal(5, length.Value);
    }

    #endregion

    #region Enum Support

    private enum TestColor { Red, Green, Blue }

    [Fact]
    public void IsEnum_EnumType_ReturnsTrue()
    {
        ICalcValue cv = CreateCalcValue(TestColor.Red);

        Assert.True(cv.IsEnum);
        Assert.False(cv.IsComplexValue);
        Assert.False(cv.IsCollection);
        Assert.False(cv.IsICalculation);
    }

    [Fact]
    public void EnumOptions_EnumType_ReturnsNames()
    {
        ICalcValue cv = CreateCalcValue(TestColor.Red);

        cv.EnumOptions.Should().BeEquivalentTo(new[] { "Red", "Green", "Blue" }, o => o.WithStrictOrdering());
    }

    [Fact]
    public void ToString_EnumType_ReturnsName()
    {
        ICalcValue cv = CreateCalcValue(TestColor.Red);

        cv.ToString().Should().Be("Red");
    }

    [Fact]
    public void TryParse_EnumType_ValidName_Succeeds()
    {
        ICalcValue cv = CreateCalcValue(TestColor.Red);

        bool result = cv.TryParse("Green");

        result.Should().BeTrue();
        cv.ValueAsObject.Should().Be(TestColor.Green);
    }

    [Fact]
    public void TryParse_EnumType_InvalidName_ReturnsFalse()
    {
        ICalcValue cv = CreateCalcValue(TestColor.Red);

        bool result = cv.TryParse("Purple");

        result.Should().BeFalse();
        cv.ValueAsObject.Should().Be(TestColor.Red);
    }

    [Fact]
    public void IsEnum_NonEnumType_ReturnsFalse()
    {
        ICalcValue cv = CreateCalcValue(42);

        Assert.False(cv.IsEnum);
        cv.EnumOptions.Should().BeEmpty();
    }

    #endregion

    #region Test Stubs

    private class ComplexStub
    {
        [InputParameter("W", "Width")]
        public double Width { get; set; } = 10.0;

        [OutputParameter("R", "Result")]
        public double Result { get; private set; } = 0.0;
    }

    private class PlainStub
    {
        public double Value1 { get; set; } = 1.0;
        public string Name { get; set; } = "test";
    }

    private class StubCalculation : Calculation
    {
        public override string CalculationTitle => "Stub";
    }

    #endregion
}
