using Scaffold.Core.CalcObjects.Profiles;

namespace Scaffold.Calculations.Sections;

public class SectionPropertiesTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new SectionPropertiesCalculation();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Section Properties Calculation", calc.ReferenceName);
        Assert.Equal("Section Properties", calc.InstanceName);
        Assert.Single(inputs);
        Assert.Equal(11, outputs.Count);
    }

    [Theory]
    [InlineData("500 x 800 cm", typeof(CalcRectangularProfile), 40)]
    [InlineData("500 800 10mm", typeof(CalcRectangularHollowProfile), 0.0256)]
    public void AreaTests(string description, Type profileType, double expectedArea)
    {
        // Assemble
        object[] parameters = new object[] { description };
        var calc = new SectionPropertiesCalculation();

        // Act
        ICalcProfile profile = (ICalcProfile)profileType.GetMethod("CreateFromDescription").Invoke(null, parameters);
        calc.Profile = profile;

        // Assert
        Assert.Equal(expectedArea, calc.Area.Value, 12);
    }
}
