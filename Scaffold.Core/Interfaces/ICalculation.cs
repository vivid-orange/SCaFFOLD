using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Interfaces;

public interface ICalculation : ICalculationStatus
{
    string InstanceName { get; set; }
    List<IOutputItem> GetFormulae();

    List<ICalcValue> GetInputs();

    List<ICalcValue> GetOutputs();

    void Calculate();
}
