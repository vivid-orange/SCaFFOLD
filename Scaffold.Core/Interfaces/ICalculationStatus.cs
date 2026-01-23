public interface ICalculationStatus
{
    /// <summary>
    /// The general name of the calculation or calcvalue this class sets out to cover, e.g. 'Punching Shear to EC2'.
    /// </summary>
    public string TypeName { get; }
    CalcStatus Status { get; }
}
