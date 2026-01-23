public interface ICalcValue : ICalculationStatus
{
    string DisplayName { get; set; }
    string Symbol { get; set; }
    bool TryParse(string strValue);
    string ValueAsString();
}
