using Scaffold.Calculations;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests
{
    public class TestCalculationTests
    {
        [Fact]
        public void TestCalculationTest1()
        {
            var reader = new CalculationReader();
            var calculation = new TestCalculation();
            // Do stuff here
            calculation.Calculate();

            Core.Models.CalculationMetadata metadata = reader.GetMetadata(calculation);
            IReadOnlyList<ICalcValue> inputs = reader.GetInputs(calculation);
            IReadOnlyList<ICalcValue> outputs = reader.GetOutputs(calculation);
            IList<Core.Interfaces.IFormula> formulae = reader.GetFormulae(calculation);

            Console.WriteLine(inputs[0]);

            // Some assertions here
        }
    }
}
