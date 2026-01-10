using FluentAssertions;
using Scaffold.Core.Abstract;

namespace Scaffold.XUnitTests.Core;

public class CalcMetadataTests
{
    private CalculationReader Reader { get; } = new();

    [Fact]
    public void CalcMetadataEmptyConstructor_Ok()
    {
        var calc = new CalcMetadataEmptyConstructor();
        Reader.Update(calc);

        calc.InstanceName.Should().Be("TypeNameSet");
        calc.ReferenceName.Should().Be("TitleSet");
    }

    [Fact]
    public void CalcMetadataTypeNameConstructor_Ok()
    {
        var calc = new CalcMetadataTypeNameConstructor();
        Reader.Update(calc);

        calc.InstanceName.Should().Be("TypeNameSet");
        calc.ReferenceName.Should().BeNull();
    }

    [Fact]
    public void CalcMetadataTypeAndTitleConstructor_Ok()
    {
        var calc = new CalcMetadataEmptyConstructor();
        Reader.Update(calc);

        calc.InstanceName.Should().Be("TypeNameSet");
        calc.ReferenceName.Should().Be("TitleSet");
    }

    [Fact]
    public void CalculationBase_FallbackValues_Used_Ok()
    {
        const string fallbackValue = "Calc Metadata Fallback";
        var calc = new CalcMetadataFallback();
        Reader.Update(calc);

        calc.InstanceName.Should().Be(fallbackValue);
        calc.ReferenceName.Should().Be(fallbackValue);
    }
}
