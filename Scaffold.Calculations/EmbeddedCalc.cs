namespace Scaffold.Calculations;

public class EmbeddedCalc : Calculation
{
    public override string CalculationTitle { get; } = "Column H/2";
    public override string EntityLabel => "Embedded calc";


    [InputParameter("H", "Column height")]
    public Length ColumnHeight { get; set; } = new Length(4.5, LengthUnit.Meter);

    [OutputParameter("H", "Reduced column height")]
    public Length ReducedColumnHeight { get; private set; } = new Length(0, LengthUnit.Meter);

    public override void Calculate()
    {
        ReducedColumnHeight = ColumnHeight / 2;
    }
}
