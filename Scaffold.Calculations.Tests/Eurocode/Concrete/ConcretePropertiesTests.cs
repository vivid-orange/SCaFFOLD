using Scaffold.Calculations.Tests;
using VividOrange.Taxonomy.Materials.StandardMaterials.En;

namespace Scaffold.Calculations.Eurocode.Concrete;

public class ConcreteMaterialPropertiesTests
{
    [Theory]
    [InlineData(EnConcreteGrade.C12_15, 12, 15, 20)]
    [InlineData(EnConcreteGrade.C16_20, 16, 20, 24)]
    [InlineData(EnConcreteGrade.C20_25, 20, 25, 28)]
    [InlineData(EnConcreteGrade.C25_30, 25, 30, 33)]
    [InlineData(EnConcreteGrade.C28_35, 28, 35, 36)]
    [InlineData(EnConcreteGrade.C30_37, 30, 37, 38)]
    [InlineData(EnConcreteGrade.C32_40, 32, 40, 40)]
    [InlineData(EnConcreteGrade.C35_45, 35, 45, 43)]
    [InlineData(EnConcreteGrade.C40_50, 40, 50, 48)]
    [InlineData(EnConcreteGrade.C45_55, 45, 55, 53)]
    [InlineData(EnConcreteGrade.C50_60, 50, 60, 58)]
    [InlineData(EnConcreteGrade.C55_67, 55, 67, 63)]
    [InlineData(EnConcreteGrade.C60_75, 60, 75, 68)]
    [InlineData(EnConcreteGrade.C70_85, 70, 85, 78)]
    [InlineData(EnConcreteGrade.C80_95, 80, 95, 88)]
    [InlineData(EnConcreteGrade.C90_105, 90, 105, 98)]
    public void ConcretePropertiesStrengthTests(
        EnConcreteGrade grade, double expFck, double expFckCube, double expFcm)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expFck, calc.fck.Megapascals);
        Assert.Equal(expFckCube, calc.fckcube.Megapascals);
        Assert.Equal(expFcm, calc.fcm.Megapascals);
    }

    [Theory]
    [InlineData(EnConcreteGrade.C12_15, 1.6, 1.1, 2.0)]
    [InlineData(EnConcreteGrade.C16_20, 1.9, 1.3, 2.5)]
    [InlineData(EnConcreteGrade.C20_25, 2.2, 1.5, 2.9)]
    [InlineData(EnConcreteGrade.C25_30, 2.6, 1.8, 3.3)]
    [InlineData(EnConcreteGrade.C30_37, 2.9, 2.0, 3.8)]
    [InlineData(EnConcreteGrade.C32_40, 3.0, 2.1, 3.9)]
    [InlineData(EnConcreteGrade.C35_45, 3.2, 2.2, 4.2)]
    [InlineData(EnConcreteGrade.C40_50, 3.5, 2.5, 4.6)]
    [InlineData(EnConcreteGrade.C45_55, 3.8, 2.7, 4.9)]
    [InlineData(EnConcreteGrade.C50_60, 4.1, 2.9, 5.3)]
    [InlineData(EnConcreteGrade.C55_67, 4.2, 3.0, 5.5)]
    [InlineData(EnConcreteGrade.C60_75, 4.4, 3.0, 5.7)] // fctk,0.05 in 1992-1-1 Table3.1 tabulated to 3.1, but formula its 3.048
    [InlineData(EnConcreteGrade.C70_85, 4.6, 3.2, 6.0)]
    [InlineData(EnConcreteGrade.C80_95, 4.8, 3.4, 6.3)]
    [InlineData(EnConcreteGrade.C90_105, 5.0, 3.5, 6.6)]
    public void ConcretePropertiesTensionTests(
       EnConcreteGrade grade, double expFctm, double expfctk005, double expfctk095)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expFctm, calc.fctm.Megapascals, expFctm.Precision());
        Assert.Equal(expfctk005, calc.fctk005.Megapascals, expfctk005.Precision());
        Assert.Equal(expfctk095, calc.fctk095.Megapascals, expfctk095.Precision());
    }

    [Theory]
    [InlineData(EnConcreteGrade.C12_15, 27)]
    [InlineData(EnConcreteGrade.C16_20, 29)]
    [InlineData(EnConcreteGrade.C20_25, 30)]
    [InlineData(EnConcreteGrade.C25_30, 31)]
    [InlineData(EnConcreteGrade.C30_37, 33)]
    [InlineData(EnConcreteGrade.C32_40, 33)]
    [InlineData(EnConcreteGrade.C35_45, 34)]
    [InlineData(EnConcreteGrade.C40_50, 35)]
    [InlineData(EnConcreteGrade.C45_55, 36)]
    [InlineData(EnConcreteGrade.C50_60, 37)]
    [InlineData(EnConcreteGrade.C55_67, 38)]
    [InlineData(EnConcreteGrade.C60_75, 39)]
    [InlineData(EnConcreteGrade.C70_85, 41)]
    [InlineData(EnConcreteGrade.C80_95, 42)]
    [InlineData(EnConcreteGrade.C90_105, 44)]
    public void ConcretePropertiesYoungsModulusTests(EnConcreteGrade grade, double expEcm)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expEcm, calc.Ecm.Gigapascals, 0);
    }

    [Theory]
    [InlineData(EnConcreteGrade.C12_15, 1.8, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C16_20, 1.9, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C20_25, 2.0, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C25_30, 2.1, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C30_37, 2.2, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C32_40, 2.2, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C35_45, 2.25, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C40_50, 2.3, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C45_55, 2.4, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData(EnConcreteGrade.C50_60, 2.46, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)] // 2.45 corrected to 2.46
    [InlineData(EnConcreteGrade.C55_67, 2.5, 3.2, 2.2, 3.1, 1.75, 1.8, 3.1)]
    [InlineData(EnConcreteGrade.C60_75, 2.6, 3.0, 2.3, 2.9, 1.6, 1.9, 2.9)]
    [InlineData(EnConcreteGrade.C70_85, 2.7, 2.8, 2.4, 2.7, 1.44, 2.0, 2.7)] // 1.45 corrected to 1.44
    [InlineData(EnConcreteGrade.C80_95, 2.8, 2.8, 2.5, 2.6, 1.4, 2.2, 2.6)]
    [InlineData(EnConcreteGrade.C90_105, 2.8, 2.8, 2.6, 2.6, 1.4, 2.3, 2.6)]
    public void ConcretePropertiesStrainTests(
        EnConcreteGrade grade, double expEpsc1, double expEpscu1, double expEpsc2, double expEpscu2,
        double expN, double expEpsc3, double expEpscu3)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expEpsc1, calc.Epsilonc1.PartsPerThousand, expEpsc1.Precision());
        Assert.Equal(expEpscu1, calc.Epsiloncu1.PartsPerThousand, expEpscu1.Precision());
        Assert.Equal(expEpsc2, calc.Epsilonc2.PartsPerThousand, expEpsc2.Precision());
        Assert.Equal(expEpscu2, calc.Epsiloncu2.PartsPerThousand, expEpscu2.Precision());
        Assert.Equal(expN, calc.n, expN.Precision());
        Assert.Equal(expEpsc3, calc.Epsilonc3.PartsPerThousand, expEpsc3.Precision());
        Assert.Equal(expEpscu3, calc.Epsiloncu3.PartsPerThousand, expEpscu3.Precision());
    }
}
