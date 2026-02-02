namespace Scaffold;

public interface ICalcValue : ICalcParameter, IFormattable
{
    bool TryParse(string strValue);
    CalcStatus Status { get; }
    List<string> Headings { get; }
    bool IsComplexValue { get; }
    bool IsCollection { get; }
    bool IsICalculation { get; }
    List<ICalcValue> GetChildInputs();
    List<ICalcValue> GetChildOutputs();
}
