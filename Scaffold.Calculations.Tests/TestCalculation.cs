using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;

namespace Scaffold.Calculations
{
    public class TestCalculation : ICalculation
    {
        public string ReferenceName { get; set; } = "";
        public string CalculationName { get; set; } = "Test Calculation";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue(@"D", "Multiplier")]
        public double Multiplier { get; set; } = 0;

        [InputCalcValue(@"F", "Force")]
        public Force Force { get; set; } = new Force(10, ForceUnit.Kilonewton);

        [OutputCalcValue(@"R", "Result")]
        public double Result { get; private set; } = 0;

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>();
        }

        public void Calculate()
        {
            Result = Force.Value * Multiplier;
        }
    }
}
