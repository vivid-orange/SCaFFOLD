namespace Scaffold.Core.Interfaces;

public interface ICalcValue : ICalculationStatus
{
    string Symbol { get; set; }
    bool TryParse(string strValue);
    string ValueAsString();
}
