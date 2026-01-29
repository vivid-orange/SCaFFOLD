namespace Scaffold.Report;

public interface IOutputItem : ICalcParameter
{
    List<IContentItem> Expressions { get; }
    string Reference { get; }
    string Conclusion { get; }
    ICalcImage Image { get; }
}
