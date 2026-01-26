public interface ICalculationStatus
{
    /// <summary>
    /// The name of the member this instance covers, e.g. 'Column C3'
    /// </summary>
    public string EntityLabel { get; }
    CalcStatus Status { get; }
}
