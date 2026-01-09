namespace Scaffold.Core.CalcValues;

public interface ICalcValue : ICalculationStatus
{
    string Symbol { get;  }
    bool SetValue(string strValue);
    string GetValue();
}
