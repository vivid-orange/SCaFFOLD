public interface ICalcValue : ICalculationStatus
{
    // string TypeName { get;  } MOVED TO ICALCULATIONSTATUS
    string Symbol { get; set; }
    bool TryParse(string strValue);
    string ValueAsString();
}
