namespace Scaffold.Core.CalcValues;

public interface IQuantity : ICalcValue
{
    string Unit { get; }
    double Value { get; set; }
}
