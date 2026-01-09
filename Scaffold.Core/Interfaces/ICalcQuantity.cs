using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Interfaces;

public interface ICalcQuantity : ICalcValue
{
    string Unit { get; }
    double Value { get; }
    public UnitsNet.IQuantity Quantity { get; set; }
}
