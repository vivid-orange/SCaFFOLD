using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Calculations.CalculationUtility;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;

public class CreateEuropeanCatalogueProfileTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfile();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Create European Catalogue Profile", calc.ReferenceName);
        Assert.Equal("European Catalogue Profile", calc.CalculationName);
        Assert.Single(outputs);
        Assert.Equal("Catalogue Profile", outputs.FirstOrDefault().TypeName);
    }

    [Fact]
    public void CalculationInputTests()
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfile();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        // TO-DO: get recursive inputs
        //Assert.Equal(expectedType, inputs[id].GetType());
        //Assert.Equal(expectedSymbol, inputs[id].Symbol);
        //Assert.Equal(expectedDisplayName, inputs[id].DisplayName);
    }

    [Fact]
    public void ChangeCatalogueAndProfileTest()
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfile();

        // Act
        calc.Profile.CatalogueType.Value = "HEB";
        calc.Profile.Output.Value = "HE200B";

        // Assert
        Assert.IsType<HE200B>(calc.Output.Value);
    }

    [Theory]
    [MemberData(nameof(CatalogueValues))]
    public void SteelCatalogueTests(string catalogue)
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfile();

        // Act
        calc.Profile.CatalogueType.Value = catalogue;

        // Assert
        Assert.Equal(catalogue, calc.Profile.CatalogueType.Value);
        Assert.True(calc.Profile.Output.Selections.Count > 10);
        Assert.NotNull(calc.Profile);

        // select each profile in catalogue type
        foreach (string prfl in calc.Profile.Output.Selections)
        {
            calc.Profile.Output.Value = prfl;
            Assert.NotNull(calc.Profile);
            Assert.Equal(prfl, calc.Profile.Output.Value);
        }
    }

    public static IEnumerable<object[]> CatalogueValues()
    {
        foreach (var catalogue in Enum.GetValues(typeof(CatalogueType)))
        {
            yield return new object[] { catalogue.ToString() };
        }
    }
}
