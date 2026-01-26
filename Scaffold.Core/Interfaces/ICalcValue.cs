public interface ICalcValue : ICalculationStatus
{
    string Symbol { get; }
    bool TryParse(string strValue);
    string ValueAsString();
    List<string> Headings => new List<string>();
}
