namespace Scaffold.Core.CalcValues;

public interface ICalcValue : ICalculationStatus
{
    string Symbol { get;  }
    bool TryParse(string strValue);
    string GetValueAsString();
}
