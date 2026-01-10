namespace Scaffold.Core.CalcValues;

public interface ICalcSIQuantity : ICalcQuantity
{
    //string Unit { get; }
    //double Value { get; }
    //public UnitsNet.IQuantity Quantity { get; set; }
    /// <summary>
    /// Provides access to generic interface
    /// </summary>
    UnitsNet.IQuantity GenericQuantity { get; }
}
