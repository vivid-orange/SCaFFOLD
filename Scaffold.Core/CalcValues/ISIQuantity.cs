using UnitsNet;

namespace Scaffold.Core.CalcValues;

public interface ISIQuantity : IQuantity
{
    UnitsNet.IQuantity GenericQuantity { get;  }
}
