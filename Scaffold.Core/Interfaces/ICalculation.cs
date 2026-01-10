using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Interfaces;

public interface ICalculation : ICalculationStatus
{
    /// <summary>
    /// The name of the instance e.g. 'Column C3'
    /// </summary>
    string InstanceName { get; set; }
    List<IOutputItem> GetFormulae();

    List<ICalcValue> GetInputs();

    List<ICalcValue> GetOutputs();

    void Calculate();
}
