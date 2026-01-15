namespace Scaffold.Calculations.Eurocode.Concrete;

public class ConcreteCreepTests
{
    [Theory]
    [InlineData(400, 800, 320000)]
    [InlineData(100, 250, 25000)]
    public void CreepAreaTests(
        double width, double length, double expArea)
    {
        // Assemble
        var calc = new CreepCalculation();

        // Act
        calc.Width = new Length(width, LengthUnit.Millimeter);
        calc.Length = new Length(length, LengthUnit.Millimeter);
        calc.Calculate();

        // Assert
        Assert.Equal(expArea, calc.Area.SquareMillimeters, 0);
    }

    [Theory]
    [InlineData(400, 800, 2400)]
    [InlineData(100, 250, 700)]
    public void CreepSectionPerimeterTests(
        double width, double length, double expPerimeter)
    {
        // Assemble
        var calc = new CreepCalculation();

        // Act
        calc.Width = new Length(width, LengthUnit.Millimeter);
        calc.Length = new Length(length, LengthUnit.Millimeter);
        calc.Calculate();

        // Assert
        Assert.Equal(expPerimeter, calc.Perimeter.Millimeters, 0);
    }

    [Theory]
    [InlineData(28, 10000000, 50, 2.291)]
    [InlineData(28, 10000000, 80, 1.702)]
    public void CreepCreepCoefficientTests(
        double time0, double time, double humidity, double expCoefficient)
    {
        // Assemble
        var calc = new CreepCalculation();

        // Act
        calc.RelativeHumidity =
            new RelativeHumidity(humidity, RelativeHumidityUnit.Percent);
        calc.Time0 = new Duration(time0, DurationUnit.Day);
        calc.Time = new Duration(time, DurationUnit.Day);
        calc.Calculate();

        // Assert
        Assert.Equal(expCoefficient, calc.CreepCoefficient, 3);
    }
}
