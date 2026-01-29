using Scaffold.Report;

namespace Scaffold;

public abstract class Calculation : ICalculation
{
    public virtual string CalculationTitle { get; } = string.Empty;
    public virtual string EntityLabel { get; } = string.Empty;
    public virtual CalcStatus Status { get; } = CalcStatus.None;
    public virtual string Symbol { get; } = string.Empty;
    public IList<IOutputItem> Formulae { get; set; } = [];

    public Calculation()
    {
        Calculate();
    }

    public virtual void Calculate() { }
    public virtual IList<IOutputItem> GetFormulae() => Formulae;
}
