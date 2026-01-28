using Scaffold.Report;

namespace Scaffold;

public interface ICalculation : ICalculationStatus
{
    /// <summary>
    /// The general name of the calculation or calcvalue this class sets out to cover, e.g. 'Punching Shear to EC2'.
    /// </summary>
    public string CalculationTitle { get; set; }

    public IList<IOutputItem> GetFormulae();

    public void Calculate();
}
