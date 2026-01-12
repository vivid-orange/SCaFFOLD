using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using VividOrange.Taxonomy.Sections.Reinforcement;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;

public class CreateRebarTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new CreateRebar();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Create Rebar", calc.ReferenceName);
        Assert.Equal("Create Rebar", calc.CalculationName);
        Assert.Single(outputs);
        Assert.Equal("Ø12 mm B500B Rebar", outputs.FirstOrDefault().DisplayName);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Grd", "Grade")]
    [InlineData(1, typeof(CalcSelectionList), "Ø", "Diameter")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new CreateRebar();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].DisplayName);
    }

    [Fact]
    public void OutputIsExpectedTypeTest()
    {
        // Assemble
        var calc = new CreateRebar();

        // Act
        calc.RebarGrade.Value = "B450A";
        calc.BarDiameter.Value = "32";

        // Assert
        Assert.IsType<CalcRebar>(calc.Output);
        Assert.Equal(EnRebarGrade.B450A, ((EnRebarMaterial)calc.Output.Material).Grade);
        Assert.Equal((double)BarDiameter.D32, calc.Output.Diameter.Millimeters);
    }

    [Fact]
    public void AdditionalConstructorTest()
    {
        // Assemble
        // Act
        var calc = new CreateRebar(BarDiameter.D6);

        // Assert
        Assert.Equal((double)BarDiameter.D6, calc.Output.Diameter.Millimeters);
    }
}
