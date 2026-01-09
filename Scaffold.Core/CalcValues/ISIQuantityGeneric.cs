using UnitsNet;

namespace Scaffold.Core.CalcValues;

public interface ISIQuantity<T> : ISIQuantity where T : UnitsNet.IQuantity
{
    T Quantity { get; }
}
