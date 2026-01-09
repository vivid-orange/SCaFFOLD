using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Calculations.CalculationUtility;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;

public class SteelCatalogueSectionTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new SteelCatalogueSection();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Steel Catalogue Section", calc.ReferenceName);
        Assert.Equal("Steel Catalogue Section", calc.CalculationName);
        //Assert.Equal(3, inputs.Count); TO-DO: make reader recursively find ICalcObjectInputs
        Assert.Single(outputs);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Grd", "Steel Grade")]
    //[InlineData(1, typeof(CalcSelectionList), "", "Profile")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new SteelCatalogueSection();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].DisplayName);
    }

    [Fact]
    public void ChangeCatalogueTest()
    {
        // Assemble
        var calc = new SteelCatalogueSection();

        // Act
        calc.Profile.Profile.CatalogueType.Value = "HEB";

        //// Assert
        Assert.Equal("HE100B", calc.Profile.Profile.Output.Value);
    }

    [Fact]
    public void ChangeCatalogueAndProfileTest()
    {
        // Assemble
        var calc = new SteelCatalogueSection();

        // Act
        calc.Profile.Profile.CatalogueType.Value = "HEB";
        calc.Profile.Profile.Output.Value = "HE200B";

        // Assert
        Assert.IsType<HE200B>(calc.Output.Profile);
    }

    [Theory]
    [MemberData(nameof(CatalogueValues))]
    public void SteelCatalogueTests(string catalogue)
    {
        // Assemble
        var calc = new SteelCatalogueSection();

        // Act
        calc.Profile.Profile.CatalogueType.Value = catalogue;

        // Assert
        Assert.Equal(catalogue, calc.Profile.Profile.CatalogueType.Value);
        Assert.True(calc.Profile.Profile.Output.Selections.Count > 10);
        Assert.NotNull(calc.Profile);

        // select each profile in catalogue type
        foreach (string prfl in calc.Profile.Profile.Output.Selections)
        {
            calc.Profile.Profile.Output.Value = prfl;
            Assert.NotNull(calc.Profile);
            Assert.Equal(prfl, calc.Profile.Profile.Output.Value);
        }
    }

    [Fact]
    public void DrawSectionTests()
    {
        // Assemble
        var calc = new SteelCatalogueSection();

        // Act
        // Assert
        Assert.NotNull(calc.GetFormulae().FirstOrDefault());
    }

    public static IEnumerable<object[]> CatalogueValues()
    {
        foreach (var catalogue in Enum.GetValues(typeof(CatalogueType)))
        {
            yield return new object[] { catalogue.ToString() };
        }
    }
}
