using Scaffold.Report;

namespace Scaffold.Calculations
{
    public class EmbeddedCalc : ICalculation
    {
        public string CalculationTitle { get; set; } = "Column H/2";

        public string EntityLabel => "Embedded calc";

        public CalcStatus Status => CalcStatus.None;

        [CalcParameter(CalcParameterType.Input, "H", "Column height")]
        public Length ColumnHeight { get; set; } = new Length(4.5, LengthUnit.Meter);

        [CalcParameter(CalcParameterType.Output, "H", "Reduced column height")]
        public Length ReducedColumnHeight { get; private set; } = new Length(0, LengthUnit.Meter);

        public void Calculate()
        {
            ReducedColumnHeight = ColumnHeight / 2;
        }
        public IList<IOutputItem> GetFormulae() => new List<IOutputItem>();
    }
}
