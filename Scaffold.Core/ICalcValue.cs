namespace Scaffold;

public interface ICalcValue : ICalcParameter
{
    bool TryParse(string strValue);
    string ValueAsString();
    CalcStatus Status { get; }
    List<string> Headings { get; }
    bool IsComplexValue { get; }
    bool IsCollection { get; }
    bool IsICalculation { get; }
    List<ICalcValue> GetChildInputs();
    List<ICalcValue> GetChildOutputs();
}
