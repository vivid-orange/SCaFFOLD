namespace Scaffold;

public interface ICalcValue : ICalcParameter, IFormattable
{
    bool TryParse(string strValue);
    CalcStatus Status { get; }
    List<string> Headings { get; }
}
