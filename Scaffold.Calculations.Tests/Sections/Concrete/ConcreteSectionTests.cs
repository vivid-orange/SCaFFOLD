using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcQuantities;

namespace Scaffold.Calculations.Sections.Concrete;

public class ConcreteSectionTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new ConcreteSection();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Concrete Section", calc.ReferenceName);
        Assert.Equal("Concrete Section", calc.CalculationName);
        //Assert.Equal(3, inputs.Count); TO-DO: make reader recursively find ICalcObjectInputs
        Assert.Single(outputs);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Grd", "Concrete Grade")]
    [InlineData(1, typeof(CalcRectangularProfile), "", "500 x 1000mm")]
    [InlineData(2, typeof(CalcLength), "Cvr", "Concrete Cover")]
    [InlineData(3, typeof(CalcRebar), "Lnk", "Link rebar")]
    [InlineData(4, typeof(CalcFaceReinforcementLayer), "Top", "Top rebars")]
    [InlineData(5, typeof(CalcFaceReinforcementLayer), "Btm", "Bottom rebars")]
    [InlineData(6, typeof(CalcFaceReinforcementLayer), "Sds", "Side rebars")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new ConcreteSection();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].TypeName);
    }

    [Fact]
    public void DrawSectionTests()
    {
        // Assemble
        var calc = new ConcreteSection();

        // Act
        // Assert
        Assert.NotNull(calc.GetFormulae().FirstOrDefault());
    }
}
