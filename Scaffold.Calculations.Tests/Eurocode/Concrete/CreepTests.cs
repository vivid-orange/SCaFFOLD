namespace Scaffold.Calculations.Eurocode.Concrete;

public class ConcreteCreepTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new Creep();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Creep", calc.ReferenceName);
        Assert.Equal("Concrete Creep", calc.InstanceName);
        Assert.Equal(6, inputs.Count);
        Assert.Equal(5, outputs.Count);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Grd", "Grade")]
    // TO-DO: a concept for organising in/outputs by Group and Position in that group
    //[InlineData(1, typeof(CalcSelectionList), "Grd", "Grade")] TO-DO add remaining inputs after list[i] is sorted
    //[InlineData(2, typeof(CalcSelectionList), "Grd", "Grade")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new Creep();

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
    //    var calc = new Creep();

    //    // Act
    //    IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

    //    // Assert
    //    Assert.Equal(expectedType, outputs[id].GetType());
    //    Assert.Equal(expectedSymbol, outputs[id].Symbol);
    //    Assert.Equal(expectedDisplayName, outputs[id].DisplayName);
    //}

    [Theory]
    [InlineData(400, 800, 320000)]
    [InlineData(100, 250, 25000)]
    public void CreepAreaTests(
        double width, double length, double expArea)
    {
        // Assemble
        var calc = new Creep();

        // Act
        calc.Width.Quantity = new Length(width, LengthUnit.Millimeter);
        calc.Length.Quantity = new Length(length, LengthUnit.Millimeter);
        calc.Calculate();

        // Assert
        Assert.Equal(expArea, calc.Area, 0);
        Assert.Equal("mm²", calc.Area.Unit);
    }

    [Theory]
    [InlineData(400, 800, 2400)]
    [InlineData(100, 250, 700)]
    public void CreepSectionPerimeterTests(
        double width, double length, double expPerimeter)
    {
        // Assemble
        var calc = new Creep();

        // Act
        calc.Width.Quantity = new Length(width, LengthUnit.Millimeter);
        calc.Length.Quantity = new Length(length, LengthUnit.Millimeter);
        calc.Calculate();

        // Assert
        Assert.Equal(expPerimeter, calc.Perimeter, 0);
        Assert.Equal("mm", calc.Perimeter.Unit);
    }

    [Theory]
    [InlineData(28, 10000000, 50, 2.291)]
    [InlineData(28, 10000000, 80, 1.702)]
    public void CreepCreepCoefficientTests(
        double time0, double time, double humidity, double expCoefficient)
    {
        // Assemble
        var calc = new Creep();

        // Act
        calc.RelativeHumidity.Quantity =
            new RelativeHumidity(humidity, RelativeHumidityUnit.Percent);
        calc.Time0.Quantity = new Duration(time0, DurationUnit.Day);
        calc.Time.Quantity = new Duration(time, DurationUnit.Day);
        calc.Calculate();

        // Assert
        Assert.Equal(expCoefficient, calc.CreepCoefficient.Value, 3);
    }
}
