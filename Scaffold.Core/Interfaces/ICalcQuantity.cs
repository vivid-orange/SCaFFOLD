public interface ICalcQuantity : ICalcValue
{
    string Unit { get; }
    double Value { get; }
    public IQuantity Quantity { get; set; }
}
