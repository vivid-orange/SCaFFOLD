using VividOrange.Taxonomy.Materials.StandardMaterials.En;

namespace Scaffold.Calculations.Eurocode.Steel;

public class SteelMaterialPropertiesTests
{
    [Theory]
    [InlineData(EnSteelGrade.S235, 235, 360)]
    [InlineData(EnSteelGrade.S275, 275, 430)]
    [InlineData(EnSteelGrade.S355, 355, 490)]
    [InlineData(EnSteelGrade.S450, 440, 550)]
    public void SteelPropertiesStrengthTests(
        EnSteelGrade grade, double expFy, double expFu)
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        calc.Grade = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expFy, calc.fy.Megapascals);
        Assert.Equal(expFu, calc.fu.Megapascals);
    }

    [Theory]
    [InlineData(EnSteelGrade.S235)]
    [InlineData(EnSteelGrade.S275)]
    [InlineData(EnSteelGrade.S355)]
    [InlineData(EnSteelGrade.S450)]
    public void SteelPropertiesStrainTests(EnSteelGrade grade)
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        calc.Grade = grade;
        calc.Calculate();

        // Assert
        double expEpsilony = calc.fy / calc.E;
        double expEpsilonu = 15 * expEpsilony;
        Assert.Equal(expEpsilony, calc.Epsilony.DecimalFractions);
        Assert.Equal(expEpsilonu, calc.Epsilonu.DecimalFractions);
    }

    [Theory]
    [InlineData(EnSteelGrade.S235)]
    [InlineData(EnSteelGrade.S275)]
    [InlineData(EnSteelGrade.S355)]
    [InlineData(EnSteelGrade.S450)]
    public void SteelPropertiesEpsilonTests(EnSteelGrade grade)
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        // Act
        calc.Grade = grade;
        calc.Calculate();

        // Assert
        double expEpsilon = Math.Sqrt(235 / calc.fy.Megapascals);
        Assert.Equal(expEpsilon, calc.Epsilon);
    }

    [Fact]
    public void SteelPropertiesElasticityTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (EnSteelGrade grade in
            Enum.GetValues(typeof(EnSteelGrade)))
        {
            // Act
            calc.Grade = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(210000, calc.E.Megapascals);
        }
    }

    [Fact]
    public void SteelPropertiesShearModulusTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (EnSteelGrade grade in
            Enum.GetValues(typeof(EnSteelGrade)))
        {
            // Act
            calc.Grade = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(80769, calc.G.Megapascals, 0);
        }
    }

    [Fact]
    public void SteelPropertiesPoissonsRatioTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (EnSteelGrade grade in
            Enum.GetValues(typeof(EnSteelGrade)))
        {
            // Act
            calc.Grade = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(0.3, calc.nu);
        }
    }

    [Fact]
    public void SteelPropertiesThermalExpansionTest()
    {
        // Assemble
        var calc = new SteelMaterialProperties();

        foreach (EnSteelGrade grade in Enum.GetValues(typeof(EnSteelGrade)))
        {
            // Act
            calc.Grade = grade;
            calc.Calculate();

            // Assert
            Assert.Equal(12 * 10 ^ -6, calc.alpha.PerKelvin);
        }
    }
}
