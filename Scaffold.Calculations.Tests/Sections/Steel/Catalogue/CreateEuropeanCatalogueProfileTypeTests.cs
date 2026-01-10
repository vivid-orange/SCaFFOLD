using Scaffold.Calculations.CalculationUtility;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;

public class CreateEuropeanCatalogueProfileTypeTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfileType();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Create European Catalogue Profile Type", calc.ReferenceName);
        Assert.Equal("European Catalogue Profile Type", calc.CalculationName);
        Assert.Single(outputs);
        Assert.Equal("Profile", outputs.FirstOrDefault().TypeName);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Typ", "Profile Type")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfileType();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].TypeName);
    }

    [Fact]
    public void ChangeCatalogueTest()
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfileType();

        // Act
        calc.CatalogueType.Value = "HEB";

        // Assert
        Assert.Equal("HEB", calc.CatalogueType.Value);
    }

    [Theory]
    [MemberData(nameof(CatalogueValues))]
    public void SteelCatalogueTests(string catalogue)
    {
        // Assemble
        var calc = new CreateEuropeanCatalogueProfileType();

        // Act
        calc.CatalogueType.Value = catalogue;

        // Assert
        Assert.Equal(catalogue, calc.CatalogueType.Value);
    }

    public static IEnumerable<object[]> CatalogueValues()
    {
        foreach (var catalogue in Enum.GetValues(typeof(CatalogueType)))
        {
            yield return new object[] { catalogue.ToString() };
        }
    }
}
