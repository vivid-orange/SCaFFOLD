using Scaffold.Calculations.Tests;

namespace Scaffold.Calculations.Eurocode.Concrete;

public class ConcreteMaterialPropertiesTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Concrete Material Properties", calc.ReferenceName);
        Assert.Equal("Concrete Material Properties", calc.InstanceName);
        Assert.Single(inputs);
        Assert.Equal(15, outputs.Count);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Grd", "Grade")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].TypeName);
    }

    // TO-DO: a concept for organising in/outputs by Group and Position in that group
    //[Theory]
    //[InlineData(0, typeof(ConcreteProperties), "CP", "Concrete Properties")]
    //public void CalculationOutputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    //{
    //    // Assemble
    //    var calc = new ConcretePropertiesCalculation();

    //    // Act
    //    IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

    //    // Assert
    //    Assert.Equal(expectedType, outputs[id].GetType());
    //    Assert.Equal(expectedSymbol, outputs[id].Symbol);
    //    Assert.Equal(expectedDisplayName, outputs[id].DisplayName);
    //}

    [Theory]
    [InlineData("C12/15", 12, 15, 20)]
    [InlineData("C16/20", 16, 20, 24)]
    [InlineData("C20/25", 20, 25, 28)]
    [InlineData("C25/30", 25, 30, 33)]
    [InlineData("C30/37", 30, 37, 38)]
    [InlineData("C32/40", 32, 40, 40)]
    [InlineData("C35/45", 35, 45, 43)]
    [InlineData("C40/50", 40, 50, 48)]
    [InlineData("C45/55", 45, 55, 53)]
    [InlineData("C50/60", 50, 60, 58)]
    [InlineData("C55/67", 55, 67, 63)]
    [InlineData("C60/75", 60, 75, 68)]
    [InlineData("C70/85", 70, 85, 78)]
    [InlineData("C80/95", 80, 95, 88)]
    [InlineData("C90/105", 90, 105, 98)]
    public void ConcretePropertiesStrengthTests(
        string grade, double expFck, double expFckCube, double expFcm)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade.Value = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expFck, calc.fck);
        Assert.Equal(expFckCube, calc.fckcube);
        Assert.Equal(expFcm, calc.fcm);
    }

    [Theory]
    [InlineData("C12/15", 1.6, 1.1, 2.0)]
    [InlineData("C16/20", 1.9, 1.3, 2.5)]
    [InlineData("C20/25", 2.2, 1.5, 2.9)]
    [InlineData("C25/30", 2.6, 1.8, 3.3)]
    [InlineData("C30/37", 2.9, 2.0, 3.8)]
    [InlineData("C32/40", 3.0, 2.1, 3.9)]
    [InlineData("C35/45", 3.2, 2.2, 4.2)]
    [InlineData("C40/50", 3.5, 2.5, 4.6)]
    [InlineData("C45/55", 3.8, 2.7, 4.9)]
    [InlineData("C50/60", 4.1, 2.9, 5.3)]
    [InlineData("C55/67", 4.2, 3.0, 5.5)]
    [InlineData("C60/75", 4.4, 3.0, 5.7)] // fctk,0.05 in 1992-1-1 Table3.1 tabulated to 3.1, but formula its 3.048
    [InlineData("C70/85", 4.6, 3.2, 6.0)]
    [InlineData("C80/95", 4.8, 3.4, 6.3)]
    [InlineData("C90/105", 5.0, 3.5, 6.6)]
    public void ConcretePropertiesTensionTests(
       string grade, double expFctm, double expfctk005, double expfctk095)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade.Value = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expFctm, calc.fctm, expFctm.Precision());
        Assert.Equal(expfctk005, calc.fctk005, expfctk005.Precision());
        Assert.Equal(expfctk095, calc.fctk095, expfctk095.Precision());
    }

    [Theory]
    [InlineData("C12/15", 27)]
    [InlineData("C16/20", 29)]
    [InlineData("C20/25", 30)]
    [InlineData("C25/30", 31)]
    [InlineData("C30/37", 33)]
    [InlineData("C32/40", 33)]
    [InlineData("C35/45", 34)]
    [InlineData("C40/50", 35)]
    [InlineData("C45/55", 36)]
    [InlineData("C50/60", 37)]
    [InlineData("C55/67", 38)]
    [InlineData("C60/75", 39)]
    [InlineData("C70/85", 41)]
    [InlineData("C80/95", 42)]
    [InlineData("C90/105", 44)]
    public void ConcretePropertiesYoungsModulusTests(string grade, double expEcm)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade.Value = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expEcm, calc.Ecm, 0);
    }

    [Theory]
    [InlineData("C12/15", 1.8, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C16/20", 1.9, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C20/25", 2.0, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C25/30", 2.1, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C30/37", 2.2, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C32/40", 2.2, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C35/45", 2.25, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C40/50", 2.3, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C45/55", 2.4, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)]
    [InlineData("C50/60", 2.46, 3.5, 2.0, 3.5, 2.0, 1.75, 3.5)] // 2.45 corrected to 2.46
    [InlineData("C55/67", 2.5, 3.2, 2.2, 3.1, 1.75, 1.8, 3.1)]
    [InlineData("C60/75", 2.6, 3.0, 2.3, 2.9, 1.6, 1.9, 2.9)]
    [InlineData("C70/85", 2.7, 2.8, 2.4, 2.7, 1.44, 2.0, 2.7)] // 1.45 corrected to 1.44
    [InlineData("C80/95", 2.8, 2.8, 2.5, 2.6, 1.4, 2.2, 2.6)]
    [InlineData("C90/105", 2.8, 2.8, 2.6, 2.6, 1.4, 2.3, 2.6)]
    public void ConcretePropertiesStrainTests(
        string grade, double expEpsc1, double expEpscu1, double expEpsc2, double expEpscu2,
        double expN, double expEpsc3, double expEpscu3)
    {
        // Assemble
        var calc = new ConcreteMaterialProperties();

        // Act
        calc.ConcreteGrade.Value = grade;
        calc.Calculate();

        // Assert
        Assert.Equal(expEpsc1, calc.Epsilonc1, expEpsc1.Precision());
        Assert.Equal(expEpscu1, calc.Epsiloncu1, expEpscu1.Precision());
        Assert.Equal(expEpsc2, calc.Epsilonc2, expEpsc2.Precision());
        Assert.Equal(expEpscu2, calc.Epsiloncu2, expEpscu2.Precision());
        Assert.Equal(expN, calc.n, expN.Precision());
        Assert.Equal(expEpsc3, calc.Epsilonc3, expEpsc3.Precision());
        Assert.Equal(expEpscu3, calc.Epsiloncu3, expEpscu3.Precision());
    }
}
