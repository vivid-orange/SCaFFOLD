namespace Scaffold.Core.Interfaces;

public interface ICalculationStatus
{
    /// <summary>
    /// The general name of the calculation this class sets out to cover, e.g. 'Punching Shear to EC2'.
    /// </summary>
    string DisplayName { get; }
    CalcStatus Status { get; }
}
