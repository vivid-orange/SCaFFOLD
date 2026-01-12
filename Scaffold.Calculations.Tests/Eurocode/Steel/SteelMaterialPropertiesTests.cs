using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using Scaffold.Core.CalcQuantities;

namespace Scaffold.Calculations.Eurocode.Steel;

public class SteelMaterialPropertiesTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Steel Material Properties", calc.ReferenceName);
        Assert.Equal("Steel Material Properties", calc.CalculationName);
        Assert.Equal(2, inputs.Count);
        Assert.Equal(10, outputs.Count);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Grd", "Grade")]
    [InlineData(1, typeof(CalcLength), "t", "Nominal thickness of the element")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].DisplayName);
    }

    // TO-DO: a concept for organising in/outputs by Group and Position in that group
    //[Theory]
    //[InlineData(0, typeof(SteelProperties), "CP", "Steel Properties")]
    //public void CalculationOutputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    //{
    //    // Assemble
    //    var calc = new SteelPropertiesCalculation();

    //    // Act
    //    IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

    //    // Assert
    //    Assert.Equal(expectedType, outputs[id].GetType());
    //    Assert.Equal(expectedSymbol, outputs[id].Symbol);
    //    Assert.Equal(expectedDisplayName, outputs[id].DisplayName);
    //}

    [Theory]
    [InlineData("S235", 235, 360)]
    [InlineData("S275", 275, 430)]
    [InlineData("S355", 355, 490)]
    [InlineData("S450", 440, 550)]
    public void SteelPropertiesStrengthTests(
        string grade, double expFy, double expFu)
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        calc.SteelGrade.Value = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expFy, calc.fy);
        Assert.Equal(expFu, calc.fu);
    }

    [Theory]
    [InlineData("S235")]
    [InlineData("S275")]
    [InlineData("S355")]
    [InlineData("S450")]
    public void SteelPropertiesStrainTests(string grade)
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        calc.SteelGrade.Value = grade;
        calc.Calculate();

        // Assert
        double expEpsilony = calc.fy / calc.E;
        double expEpsilonu = 15 * expEpsilony;
        Assert.Equal(expEpsilony, calc.Epsilony);
        Assert.Equal(expEpsilonu, calc.Epsilonu);
    }

    [Theory]
    [InlineData("S235")]
    [InlineData("S275")]
    [InlineData("S355")]
    [InlineData("S450")]
    public void SteelPropertiesEpsilonTests(string grade)
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        calc.SteelGrade.Value = grade;
        calc.Calculate();

        // Assert
        double expEpsilon = Math.Sqrt(235 / calc.fy);
        Assert.Equal(expEpsilon, (double)calc.Epsilon);
    }

    [Fact]
    public void SteelPropertiesElasticityTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (string grade in
            Enum.GetValues(typeof(EnSteelGrade)).Cast<EnSteelGrade>().Select(v => v.ToString()).ToList())
        {
            // Act
            calc.SteelGrade.Value = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(210000, (double)calc.E);
        }
    }

    [Fact]
    public void SteelPropertiesShearModulusTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (string grade in
            Enum.GetValues(typeof(EnSteelGrade)).Cast<EnSteelGrade>().Select(v => v.ToString()).ToList())
        {
            // Act
            calc.SteelGrade.Value = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(80769, (double)calc.G, 0);
        }
    }

    [Fact]
    public void SteelPropertiesPoissonsRatioTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (string grade in
            Enum.GetValues(typeof(EnSteelGrade)).Cast<EnSteelGrade>().Select(v => v.ToString()).ToList())
        {
            // Act
            calc.SteelGrade.Value = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(0.3, (double)calc.nu);
        }
    }

    [Fact]
    public void SteelPropertiesThermalExpansionTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (string grade in
            Enum.GetValues(typeof(EnSteelGrade)).Cast<EnSteelGrade>().Select(v => v.ToString()).ToList())
        {
            // Act
            calc.SteelGrade.Value = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(12 * 10 ^ -6, (double)calc.alpha);
        }
    }
}
