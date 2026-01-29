namespace Scaffold.Report;

public interface IOutputItem : ICalculationStatus
{
    List<IContentItem> Expressions { get; }
    string Reference { get; }
    string Conclusion { get; }
    ICalcImage Image { get; }
}
