using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

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
        public CalcForce Force { get; set; } = new CalcForce(10, "Force", "F");

        [OutputCalcValue(@"R", "Result")]
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
