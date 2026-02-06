namespace Scaffold;

public interface ICalcValue : ICalcParameter, IFormattable
{
    bool TryParse(string strValue);
    CalcStatus Status { get; }
    List<string> Headings { get; }
    bool IsComplexValue { get; }
    bool IsCollection { get; }
    bool IsICalculation { get; }
    bool IsEnum { get; }
    IReadOnlyList<string> EnumOptions { get; }
    bool IsQuantity { get; }
    IReadOnlyList<string> UnitOptions { get; }
    int SelectedUnitIndex { get; }
    bool TrySetUnitByIndex(int unitIndex);
    string NumericValueString { get; }
    List<ICalcValue> GetChildInputs();
    List<ICalcValue> GetChildOutputs();
    object ValueAsObject { get; }
}

public interface ICalcValue<T> : ICalcValue
{
    T Value { get; set; }
}
