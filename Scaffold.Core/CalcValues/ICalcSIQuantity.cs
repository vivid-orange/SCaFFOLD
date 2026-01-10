namespace Scaffold.Core.CalcValues;

public interface ICalcSIQuantity : ICalcQuantity
{
    //string Unit { get; }
    //double Value { get; }
    //public UnitsNet.IQuantity Quantity { get; set; }

    UnitsNet.IQuantity GenericQuantity { get; }
}
