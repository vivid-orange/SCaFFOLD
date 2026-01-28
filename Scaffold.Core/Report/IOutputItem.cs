namespace Scaffold.Report;

public interface IOutputItem : ICalculationStatus
{
    List<IContentItem> Expressions { get; }
    string Reference { get; }
    //string Narrative { get; }
    string Conclusion { get; }
    ICalcImage Image { get; }
}
