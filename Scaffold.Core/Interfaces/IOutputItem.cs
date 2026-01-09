namespace Scaffold.Core.Interfaces;

public interface IOutputItem : ICalculationStatus
{
    List<string> Expressions { get; }
    string Reference { get; }
    //string Narrative { get; }
    string Conclusion { get; }
    ICalcImage Image { get; }
}
