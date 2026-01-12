using VividOrange.Taxonomy.Sections;
using VividOrange.Taxonomy.Sections.Reinforcement;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcQuantities;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;

public class CreateFaceReinforcementLayerByCountTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new CreateFaceReinforcementLayerByCount();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Create Face Reinforcement Layer By Count", calc.ReferenceName);
        Assert.Equal("Create Face Reinforcement Layer By Count", calc.CalculationName);
        Assert.Single(outputs);
        Assert.Equal("Bottom Rebar 4 No.", outputs.FirstOrDefault().DisplayName);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Face", "Section Face")]
    [InlineData(1, typeof(CalcRebar), "Bar", "Rebar")]
    [InlineData(2, typeof(CalcInt), "No.", "Number of Bars")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new CreateFaceReinforcementLayerByCount();

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
        var calc = new CreateFaceReinforcementLayerByCount();

        // Act
        calc.SectionFace.Value = "Top";
        calc.Rebar = new CreateRebar(BarDiameter.D16);
        calc.Count = 6;

        // Assert
        Assert.IsType<CalcFaceReinforcementLayer>(calc.Output);
        Assert.IsType<ReinforcementLayoutByCount>(calc.Output.Layout);
        Assert.Equal(SectionFace.Top, calc.Output.Face);
        Assert.Equal((double)BarDiameter.D16, calc.Output.Layout.Rebar.Diameter.Millimeters);
        Assert.Equal(6, ((ReinforcementLayoutByCount)calc.Output.Layout).NumberOfBars);
    }

    [Theory]
    [InlineData(SectionFace.Bottom)]
    [InlineData(SectionFace.Top)]
    [InlineData(SectionFace.Left)]
    [InlineData(SectionFace.Right)]
    public void AdditionalConstructorTest(SectionFace face)
    {
        // Assemble
        // Act
        var calc = new CreateFaceReinforcementLayerByCount(face);

        // Assert
        Assert.Equal(face, calc.Output.Face);
    }
}
