public interface ICalcValue : ICalculationStatus
{
    string Symbol { get; }
    bool TryParse(string strValue);
    string ValueAsString();
    List<string> Headings { get; }
    bool IsComplexValue { get; }
    bool IsCollection { get; }
    bool IsICalculation { get; }
    List<ICalcValue> GetChildInputs();
    List<ICalcValue> GetChildOutputs();
}
