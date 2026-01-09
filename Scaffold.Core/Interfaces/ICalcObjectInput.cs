using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Interfaces;

public interface ICalcObjectInput<T> : ICalculation where T : ICalcValue
{
    T Output { get; }
}
