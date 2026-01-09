using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Interfaces;

public interface ICalculation : ICalculationStatus
{
    /// <summary>
    /// The name of the member this instance covers, e.g. 'Column C3'
    /// </summary>
    public string DisplayName { get; set; } // ReferenceName

    List<IOutputItem> GetFormulae();

    List<ICalcValue> GetInputs();

    List<ICalcValue> GetOutputs();

    void Calculate();
}
