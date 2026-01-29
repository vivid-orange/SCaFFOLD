namespace Scaffold;

public interface ICalcParameter
{
    /// <summary>
    /// The display name of the member this instance covers, e.g. 'Column C3'
    /// </summary>
    public string EntityLabel { get; }

    /// <summary>
    /// The symbol, acronym or greek letter, of the member this instance covers, e.g. 'C3'
    /// </summary>
    string Symbol { get; }
}
