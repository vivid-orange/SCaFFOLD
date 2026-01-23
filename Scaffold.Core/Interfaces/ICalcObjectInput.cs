public interface ICalcObjectInput<T> : ICalculation where T : ICalcValue
{
    T Output { get; }
}
