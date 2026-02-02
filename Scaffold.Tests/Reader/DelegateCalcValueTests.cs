using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Scaffold.Reader;
using UnitsNet;
using UnitsNet.Units;

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
        var cv = Create(length);

        string result = cv.ToString("F2", CultureInfo.InvariantCulture);

        result.Should().Be(length.ToString("F2", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ToString_Collection_ReturnsCountSummary()
    {
        var list = new List<int> { 1, 2, 3 };
        var cv = Create(list);

        string result = cv.ToString(null, CultureInfo.InvariantCulture);

        result.Should().Be("List`1 (3 items)");
    }

    [Fact]
    public void ToString_Override_UsesInvariantCulture()
    {
        var length = new Length(1.5, LengthUnit.Meter);
        var cv = Create(length);

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
        var cv = Create(new Length(0, LengthUnit.Meter));

        bool result = cv.TryParse("5 m");

        result.Should().BeTrue();
        cv.Value.Should().Be(new Length(5, LengthUnit.Meter));
    }

    [Fact]
    public void TryParse_IQuantity_BareNumber_PreservesUnit()
    {
        var cv = Create(new Length(1, LengthUnit.Millimeter));

        bool result = cv.TryParse("10");

        result.Should().BeTrue();
        cv.Value.Should().Be(new Length(10, LengthUnit.Millimeter));
    }

    [Fact]
    public void TryParse_IQuantity_InvalidString_ReturnsFalse()
    {
        var original = new Length(5, LengthUnit.Meter);
        var cv = Create(original);

        bool result = cv.TryParse("not a number");

        result.Should().BeFalse();
        cv.Value.Should().Be(original);
    }

    #endregion

    #region TryParse — IParsable / TypeConverter

    [Fact]
    public void TryParse_Int_Succeeds()
    {
        var cv = Create(0);

        bool result = cv.TryParse("42");

        result.Should().BeTrue();
        cv.Value.Should().Be(42);
    }

    [Fact]
    public void TryParse_Double_Succeeds()
    {
        var cv = Create(0.0);

        bool result = cv.TryParse("3.14");

        result.Should().BeTrue();
        cv.Value.Should().BeApproximately(3.14, 0.001);
    }

    [Fact]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        var cv = Create(0);

        bool result = cv.TryParse("abc");

        result.Should().BeFalse();
        cv.Value.Should().Be(0);
    }

    #endregion

    #region TryParse — null setter

    [Fact]
    public void TryParse_NullSetter_ReturnsFalse()
    {
        var cv = CreateReadOnly(42);

        bool result = cv.TryParse("99");

        result.Should().BeFalse();
    }

    #endregion

    #region TryParse — IQuantity Exception Handling

    [Fact]
    public void TryParse_IQuantity_UnitsNetException_TriesBareNumber()
    {
        var cv = Create(new Length(5, LengthUnit.Meter));

        bool result = cv.TryParse("invalid unit");

        result.Should().BeFalse();
        cv.Value.Should().Be(new Length(5, LengthUnit.Meter));
    }

    #endregion

    #region TryParse — TypeConverter Edge Cases

    [Fact]
    public void TryParse_TypeConverterThrows_ReturnsFalse()
    {
        var cv = Create(new Guid("12345678-1234-1234-1234-123456789012"));

        // Try to parse with malformed GUID format
        bool result = cv.TryParse("not-a-guid");

        result.Should().BeFalse();
    }

    [Fact]
    public void TryParse_NoConverterAvailable_ReturnsFalse()
    {
        // Create a value type with no TypeConverter
        var cv = Create(new object());

        bool result = cv.TryParse("anything");

        result.Should().BeFalse();
    }

    #endregion

    #region ToString — Edge Cases

    [Fact]
    public void ToString_StringType_DoesNotShowAsCollection()
    {
        var cv = Create("hello world");

        string result = cv.ToString();

        result.Should().Be("hello world");
    }

    [Fact]
    public void ToString_EmptyCollection_ReturnsZeroItems()
    {
        var list = new List<int>();
        var cv = Create(list);

        string result = cv.ToString();

        result.Should().Be("List`1 (0 items)");
    }

    [Fact]
    public void ToString_NonFormattable_UsesDefaultToString()
    {
        var obj = new object();
        var cv = Create(obj);

        string result = cv.ToString();

        result.Should().Be(obj.ToString());
    }

    [Fact]
    public void ToString_WithFormat_PassesToIFormattable()
    {
        var length = new Length(1.23456, LengthUnit.Meter);
        var cv = Create(length);

        string result = cv.ToString("F1", CultureInfo.InvariantCulture);

        result.Should().Contain("1.2");
    }

    #endregion

    #region Value Property

    [Fact]
    public void Value_Setter_InvokesAction()
    {
        var cv = Create(0);

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
        var cv = CreateReadOnly(42);

        // Should not throw
        cv.Value = 99;

        cv.Value.Should().Be(42);
    }

    #endregion

    #region Constructor Edge Cases

    [Fact]
    public void Constructor_NullHeadings_CreatesEmptyList()
    {
        var cv = Create(42, headings: null);

        cv.Headings.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_NonNullHeadings_CreatesListWithItems()
    {
        var headings = new[] { "Group1", "Group2" };
        var cv = Create(42, headings: headings);

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
        var cv = Create(Guid.Empty);

        bool result = cv.TryParse("12345678-1234-1234-1234-123456789abc");

        result.Should().BeTrue();
        cv.Value.Should().Be(new Guid("12345678-1234-1234-1234-123456789abc"));
    }

    [Fact]
    public void TryParse_IParsableType_InvalidFormat_ReturnsFalse()
    {
        var cv = Create(Guid.Empty);

        bool result = cv.TryParse("not-a-guid-format");

        result.Should().BeFalse();
        cv.Value.Should().Be(Guid.Empty);
    }

    #endregion
}
