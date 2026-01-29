using Scaffold.Report;

namespace Scaffold.Calculations
{
    public class TestCalculation : ICalculation
    {
        public string CalculationTitle { get; set; } = "";
        public string EntityLabel { get; set; } = "Test Calculation";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputParameter(@"D", "Multiplier")]
        public double Multiplier { get; set; } = 0;

        [InputParameter(@"F", "Force")]
        public Force Force { get; set; } = new Force(10, ForceUnit.Kilonewton);

        [OutputParameter(@"R", "Result")]
        public double Result { get; private set; } = 0;

        public IList<IOutputItem> GetFormulae()
        {
            return new List<IOutputItem>();
        }

        public void Calculate()
        {
            Result = Force.Value * Multiplier;
        }
    }
}
