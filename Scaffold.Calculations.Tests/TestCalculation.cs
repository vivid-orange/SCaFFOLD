using Scaffold.Core;

namespace Scaffold.Calculations
{
    public class TestCalculation : ICalculation
    {
        public string CalculationTitle { get; set; } = "";
        public string EntityLabel { get; set; } = "Test Calculation";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue(@"D", "Multiplier")]
        public double Multiplier { get; set; } = 0;

        [InputCalcValue(@"F", "Force")]
        public Force Force { get; set; } = new Force(10, ForceUnit.Kilonewton);

        [OutputCalcValue(@"R", "Result")]
        public double Result { get; private set; } = 0;

        public IList<IContentItem> GetFormulae()
        {
            return new List<IContentItem>();
        }

        public void Calculate()
        {
            Result = Force.Value * Multiplier;
        }
    }
}
