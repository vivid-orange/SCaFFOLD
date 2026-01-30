namespace Scaffold.Reader.Utility;

/// <summary>
/// Holds the cached lists for a specific ICalculation instance
/// </summary>
internal class InstanceCache
{
    public List<ICalcValue> Inputs { get; set; }
    public List<ICalcValue> Outputs { get; set; }
}
