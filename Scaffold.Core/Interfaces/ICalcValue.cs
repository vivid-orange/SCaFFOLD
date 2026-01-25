public interface ICalcValue : ICalculationStatus
{
    // string TypeName { get;  } MOVED TO ICALCULATIONSTATUS
    string Symbol { get; }
    bool TryParse(string strValue);
    string ValueAsString();

    List<string> Headings => new List<string>();
}
