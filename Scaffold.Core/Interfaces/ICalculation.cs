public interface ICalculation : ICalculationStatus
{
    /// <summary>
    /// The general name of the calculation this class sets out to cover, e.g. 'Punching Shear to EC2'.
    /// By default, this will use CalcNameAttribute if it exists, otherwise it will use the value you specify.
    /// If you do not specify a value, the class name will be used.
    /// </summary>
    public string CalculationName { get; set; }

    /// <summary>
    /// The name of the member this instance covers, e.g. 'Column C3'
    /// </summary>
    public string ReferenceName { get; set; }

    public IList<IFormula> GetFormulae();

    public void Calculate();
}
