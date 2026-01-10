using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcQuantities;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;

public class CreateFaceReinforcementLayerBySpacingTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new CreateFaceReinforcementLayerBySpacing();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Create Face Reinforcement Layer By Spacing", calc.ReferenceName);
        Assert.Equal("Create Face Reinforcement Layer By Spacing", calc.CalculationName);
        Assert.Single(outputs);
        Assert.Equal("Bottom Rebar @ 200 mm c/c", outputs.FirstOrDefault().TypeName);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Face", "Section Face")]
    [InlineData(1, typeof(CalcRebar), "Bar", "Rebar")]
    [InlineData(2, typeof(CalcLength), "a", "Max. rebar spacing")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new CreateFaceReinforcementLayerBySpacing();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].TypeName);
    }

    [Fact]
    public void OutputIsExpectedTypeTest()
    {
        // Assemble
        var calc = new CreateFaceReinforcementLayerBySpacing();

        // Act
        calc.SectionFace.Value = "Top";
        calc.Rebar = new CreateRebar(BarDiameter.D40);
        calc.Spacing = new CalcLength(150, LengthUnit.Millimeter, "minSpacing", "a");

        // Assert
        Assert.IsType<CalcFaceReinforcementLayer>(calc.Output);
        Assert.IsType<ReinforcementLayoutBySpacing>(calc.Output.Layout);
        Assert.Equal(SectionFace.Top, calc.Output.Face);
        Assert.Equal((double)BarDiameter.D40, calc.Output.Layout.Rebar.Diameter.Millimeters);
        Assert.Equal(150, ((ReinforcementLayoutBySpacing)calc.Output.Layout).MaximumSpacing.Millimeters);
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
        var calc = new CreateFaceReinforcementLayerBySpacing(face);

        // Assert
        Assert.Equal(face, calc.Output.Face);
    }
}
