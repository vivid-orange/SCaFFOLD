namespace Scaffold.Core.CalcValues;

public interface ICalcQuantity : ICalcValue
{
    string Unit { get; }
    double Value { get; set; }
}
