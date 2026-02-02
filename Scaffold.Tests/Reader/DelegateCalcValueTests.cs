using System.Globalization;
using FluentAssertions;
using Scaffold.Reader;
using UnitsNet;

namespace Scaffold.Tests.Reader;

public class DelegateCalcValueTests
{
    #region Helpers

    private static DelegateCalcValue<T> Create<T>(T initial, Action<T>? setter = null, string[]? headings = null)
    {
        T value = initial;
        return new DelegateCalcValue<T>(
            getter: () => value,
            setter: setter ?? (v => value = v),
            symbol: "x",
            displayName: "Test",
            headings: headings ?? Enumerable.Empty<string>());
    }

    private static DelegateCalcValue<T> CreateReadOnly<T>(T initial)
    {
        T value = initial;
        return new DelegateCalcValue<T>(
            getter: () => value,
            setter: null!,
            symbol: "x",
            displayName: "Test",
            headings: Enumerable.Empty<string>());
    }

    #endregion

    #region IFormattable / ToString

    [Fact]
    public void ToString_DelegatesToUnderlyingIFormattable()
    {
        var length = new Length(3.14159, LengthUnit.Meter);
        DelegateCalcValue<Length> cv = Create(length);

        string result = cv.ToString("F2", CultureInfo.InvariantCulture);

        result.Should().Be(length.ToString("F2", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ToString_Collection_ReturnsCountSummary()
    {
        var list = new List<int> { 1, 2, 3 };
        DelegateCalcValue<List<int>> cv = Create(list);

        string result = cv.ToString(null, CultureInfo.InvariantCulture);

        result.Should().Be("List`1 (3 items)");
    }

    [Fact]
    public void ToString_Override_UsesInvariantCulture()
    {
        var length = new Length(1.5, LengthUnit.Meter);
        DelegateCalcValue<Length> cv = Create(length);

        string overrideResult = cv.ToString();
        string formattableResult = cv.ToString(null, CultureInfo.InvariantCulture);

        overrideResult.Should().Be(formattableResult);
    }

    [Fact]
    public void ToString_NullValue_ReturnsEmpty()
    {
        string? val = null;
        var cv = new DelegateCalcValue<string?>(
            getter: () => val,
            setter: v => val = v,
            symbol: "x",
            displayName: "Test",
            headings: Enumerable.Empty<string>());

        cv.ToString().Should().Be(string.Empty);
    }

    #endregion

    #region TryParse — IQuantity

    [Fact]
    public void TryParse_IQuantity_FullUnitString_Succeeds()
    {
        DelegateCalcValue<Length> cv = Create(new Length(0, LengthUnit.Meter));

        bool result = cv.TryParse("5 m");

        result.Should().BeTrue();
        cv.Value.Should().Be(new Length(5, LengthUnit.Meter));
    }

    [Fact]
    public void TryParse_IQuantity_BareNumber_PreservesUnit()
    {
        DelegateCalcValue<Length> cv = Create(new Length(1, LengthUnit.Millimeter));

        bool result = cv.TryParse("10");

        result.Should().BeTrue();
        cv.Value.Should().Be(new Length(10, LengthUnit.Millimeter));
    }

    [Fact]
    public void TryParse_IQuantity_InvalidString_ReturnsFalse()
    {
        var original = new Length(5, LengthUnit.Meter);
        DelegateCalcValue<Length> cv = Create(original);

        bool result = cv.TryParse("not a number");

        result.Should().BeFalse();
        cv.Value.Should().Be(original);
    }

    #endregion

    #region TryParse — IParsable / TypeConverter

    [Fact]
    public void TryParse_Int_Succeeds()
    {
        DelegateCalcValue<int> cv = Create(0);

        bool result = cv.TryParse("42");

        result.Should().BeTrue();
        cv.Value.Should().Be(42);
    }

    [Fact]
    public void TryParse_Double_Succeeds()
    {
        DelegateCalcValue<double> cv = Create(0.0);

        bool result = cv.TryParse("3.14");

        result.Should().BeTrue();
        cv.Value.Should().BeApproximately(3.14, 0.001);
    }

    [Fact]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        DelegateCalcValue<int> cv = Create(0);

        bool result = cv.TryParse("abc");

        result.Should().BeFalse();
        cv.Value.Should().Be(0);
    }

    #endregion

    #region TryParse — null setter

    [Fact]
    public void TryParse_NullSetter_ReturnsFalse()
    {
        DelegateCalcValue<int> cv = CreateReadOnly(42);

        bool result = cv.TryParse("99");

        result.Should().BeFalse();
    }

    #endregion

    #region TryParse — IQuantity Exception Handling

    [Fact]
    public void TryParse_IQuantity_UnitsNetException_TriesBareNumber()
    {
        DelegateCalcValue<Length> cv = Create(new Length(5, LengthUnit.Meter));

        bool result = cv.TryParse("invalid unit");

        result.Should().BeFalse();
        cv.Value.Should().Be(new Length(5, LengthUnit.Meter));
    }

    #endregion

    #region TryParse — TypeConverter Edge Cases

    [Fact]
    public void TryParse_TypeConverterThrows_ReturnsFalse()
    {
        DelegateCalcValue<Guid> cv = Create(new Guid("12345678-1234-1234-1234-123456789012"));

        // Try to parse with malformed GUID format
        bool result = cv.TryParse("not-a-guid");

        result.Should().BeFalse();
    }

    [Fact]
    public void TryParse_NoConverterAvailable_ReturnsFalse()
    {
        // Create a value type with no TypeConverter
        DelegateCalcValue<object> cv = Create(new object());

        bool result = cv.TryParse("anything");

        result.Should().BeFalse();
    }

    #endregion

    #region ToString — Edge Cases

    [Fact]
    public void ToString_StringType_DoesNotShowAsCollection()
    {
        DelegateCalcValue<string> cv = Create("hello world");

        string result = cv.ToString();

        result.Should().Be("hello world");
    }

    [Fact]
    public void ToString_EmptyCollection_ReturnsZeroItems()
    {
        var list = new List<int>();
        DelegateCalcValue<List<int>> cv = Create(list);

        string result = cv.ToString();

        result.Should().Be("List`1 (0 items)");
    }

    [Fact]
    public void ToString_NonFormattable_UsesDefaultToString()
    {
        var obj = new object();
        DelegateCalcValue<object> cv = Create(obj);

        string result = cv.ToString();

        result.Should().Be(obj.ToString());
    }

    [Fact]
    public void ToString_WithFormat_PassesToIFormattable()
    {
        var length = new Length(1.23456, LengthUnit.Meter);
        DelegateCalcValue<Length> cv = Create(length);

        string result = cv.ToString("F1", CultureInfo.InvariantCulture);

        result.Should().Contain("1.2");
    }

    #endregion

    #region Value Property

    [Fact]
    public void Value_Setter_InvokesAction()
    {
        DelegateCalcValue<int> cv = Create(0);

        cv.Value = 42;

        cv.Value.Should().Be(42);
    }

    [Fact]
    public void Value_Getter_InvokesFuncMultipleTimes()
    {
        int callCount = 0;
        int value = 10;
        var cv = new DelegateCalcValue<int>(
            getter: () => { callCount++; return value; },
            setter: v => value = v,
            symbol: "x",
            displayName: "Test",
            headings: Enumerable.Empty<string>());

        _ = cv.Value;
        _ = cv.Value;

        callCount.Should().Be(2);
    }

    [Fact]
    public void Value_SetterWithNullAction_DoesNotThrow()
    {
        DelegateCalcValue<int> cv = CreateReadOnly(42);

        // Should not throw
        cv.Value = 99;

        cv.Value.Should().Be(42);
    }

    #endregion

    #region Constructor Edge Cases

    [Fact]
    public void Constructor_NullHeadings_CreatesEmptyList()
    {
        DelegateCalcValue<int> cv = Create(42, headings: null);

        cv.Headings.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_NonNullHeadings_CreatesListWithItems()
    {
        var headings = new[] { "Group1", "Group2" };
        DelegateCalcValue<int> cv = Create(42, headings: headings);

        cv.Headings.Should().HaveCount(2);
        cv.Headings.Should().ContainInOrder("Group1", "Group2");
    }

    [Fact]
    public void Constructor_NullDisplayName_UsesTypeName()
    {
        var cv = new DelegateCalcValue<int>(
            getter: () => 42,
            setter: v => { },
            symbol: "x",
            displayName: null,
            headings: Enumerable.Empty<string>());

        cv.EntityLabel.Should().Be("Int32");
    }

    #endregion

    #region TryParse — IParsable Path

    [Fact]
    public void TryParse_IParsableType_Succeeds()
    {
        DelegateCalcValue<Guid> cv = Create(Guid.Empty);

        bool result = cv.TryParse("12345678-1234-1234-1234-123456789abc");

        result.Should().BeTrue();
        cv.Value.Should().Be(new Guid("12345678-1234-1234-1234-123456789abc"));
    }

    [Fact]
    public void TryParse_IParsableType_InvalidFormat_ReturnsFalse()
    {
        DelegateCalcValue<Guid> cv = Create(Guid.Empty);

        bool result = cv.TryParse("not-a-guid-format");

        result.Should().BeFalse();
        cv.Value.Should().Be(Guid.Empty);
    }

    #endregion

    #region Type Flags

    [Theory]
    [InlineData(42)]
    [InlineData("hello")]
    [InlineData(3.14)]
    public void TypeFlags_Primitive_AllFalse(object primitive)
    {
        // Arrange
        DelegateCalcValue<object> cv = Create(primitive);

        // Assert
        Assert.False(cv.IsICalculation);
        Assert.False(cv.IsCollection);
        Assert.False(cv.IsComplexValue);
    }

    [Fact]
    public void TypeFlags_ListOfInt_IsCollectionTrue()
    {
        // Arrange
        DelegateCalcValue<List<int>> cv = Create(new List<int> { 1, 2 });

        // Assert
        Assert.False(cv.IsICalculation);
        Assert.True(cv.IsCollection);
    }

    [Fact]
    public void TypeFlags_Calculation_IsICalculationTrue()
    {
        // Arrange
        var calc = new StubCalculation();
        DelegateCalcValue<StubCalculation> cv = Create<StubCalculation>(calc);

        // Assert
        Assert.True(cv.IsICalculation);
    }

    #endregion

    #region CheckIfComplex

    [Fact]
    public void CheckIfComplex_TypeWithCalcParameterAttribute_IsComplexTrue()
    {
        // Arrange — ComplexStub has [InputParameter] on a property
        DelegateCalcValue<ComplexStub> cv = Create(new ComplexStub());

        // Assert
        Assert.True(cv.IsComplexValue);
    }

    [Fact]
    public void CheckIfComplex_PlainClassNoAttributes_IsComplexFalse()
    {
        // Arrange
        DelegateCalcValue<PlainStub> cv = Create(new PlainStub());

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
        var cv = new DelegateCalcValue<string?>(
            () => val, v => val = v, "x", "Test", Enumerable.Empty<string>());

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
        var cv = new DelegateCalcValue<string?>(
            () => val, v => val = v, "x", "Test", Enumerable.Empty<string>());

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
        DelegateCalcValue<ComplexStub> cv = Create(obj);

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
        DelegateCalcValue<ComplexStub> cv = Create(obj);

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
        DelegateCalcValue<Length> cv = Create(new Length(1, LengthUnit.Meter));

        // Act
        bool result = cv.TryParse("5 mm");

        // Assert
        Assert.True(result);
        Assert.Equal(LengthUnit.Millimeter, ((Length)(object)cv.Value).Unit);
        Assert.Equal(5, ((Length)(object)cv.Value).Value);
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
