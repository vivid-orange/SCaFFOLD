namespace Scaffold.Report;

public class OutputItem : IOutputItem
{
    public OutputItem() { }

    public OutputItem(string reference, string conclusion, IContentItem expression,
        CalcStatus status = CalcStatus.None)
    {
        Reference = reference;
        Conclusion = conclusion;
        Expressions.Add(expression);
        Status = status;
    }

    public string EntityLabel { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public List<IContentItem> Expressions { get; set; } = [];
    public string Reference { get; set; } = string.Empty;
    public string Conclusion { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;
    public ICalcImage Image { get; set; }

    public OutputItem WithConclusion(string conclusion)
    {
        Conclusion = conclusion;
        return this;
    }

    public OutputItem WithStatus(CalcStatus status)
    {
        Status = status;
        return this;
    }

    public OutputItem WithReference(string reference)
    {
        Reference = reference;
        return this;
    }

    public OutputItem AddExpression(IContentItem expression)
    {
        Expressions.Add(expression);
        return this;
    }

    public OutputItem AddExpressions(IEnumerable<IContentItem> expressions)
    {
        Expressions.AddRange(expressions);
        return this;
    }

    public OutputItem AddImage(ICalcImage image)
    {
        Image = image;
        return this;
    }
}
