namespace Scaffold.Core.Models;

public class Formula : IOutputItem
{
    public List<string> Expressions { get; set; } = new List<string>();
    public string Reference { get; set; } = "";
    public string Narrative { get; set; } = "";
    public string Conclusion { get; set; } = "";
    public CalcStatus Status { get; set; } = CalcStatus.None;
    public ICalcImage Image { get; set; }

    public string DisplayName { get; }

    public Formula() { }

    public Formula(string reference, string narrative, string conclusion, string expression,
        CalcStatus status = CalcStatus.None)
    {
        Reference = reference;
        Narrative = narrative;
        Conclusion = conclusion;
        Expressions.Add(expression);
        Status = status;
    }

    public static Formula New(string narrative)
    {
        return new Formula { Narrative = narrative };
    }

    public Formula WithConclusion(string conclusion)
    {
        Conclusion = conclusion;
        return this;
    }

    public Formula WithStatus(CalcStatus status)
    {
        Status = status;
        return this;
    }

    public Formula WithReference(string reference)
    {
        Reference = reference;
        return this;
    }

    public Formula AddExpression(string expression)
    {
        Expressions.Add(expression);
        return this;
    }

    public Formula AddExpressions(IEnumerable<string> expressions)
    {
        Expressions.AddRange(expressions);
        return this;
    }

    public Formula SetImage(ICalcImage image)
    {
        Image = image;
        return this;
    }
}
