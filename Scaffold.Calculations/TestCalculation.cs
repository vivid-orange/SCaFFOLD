using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Calculations
{
    public class TestCalculation : ICalculation
    {
        public string DisplayName { get; set; } = "";
        public string CalculationName { get; set; } = "Test Calculation";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue(@"D", "Multiplier")]
        public double Multiplier { get; set; } = 0;

        [InputCalcValue(@"F", "Force")]
        public CalcForce Force { get; set; } = new CalcForce(10, "Force", "F");

        [OutputCalcValue(@"R", "Result")]
        public double Result { get; private set; } = 0;

        public List<IOutputItem> GetFormulae()
        {
            return new List<IOutputItem>();
        }

        public void Calculate()
        {
            Result = Force.Value * Multiplier;
        }

        public List<ICalcValue> GetInputs() => throw new System.NotImplementedException();
        public List<ICalcValue> GetOutputs() => throw new System.NotImplementedException();
    }
}
