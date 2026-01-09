using Scaffold.Core.Enums;
using Scaffold.Core.Images;
using System.Collections.ObjectModel;

namespace Scaffold.Core;

public class OutputItem : IOutputItem
{
    public OutputItem() { }

    public OutputItem(string reference, string narrative, string conclusion, IExpression expression,
        CalcStatus status = CalcStatus.None)
    {
        Reference = reference;
        Narrative = narrative;
        Conclusion = conclusion;
        Expressions.Add(expression);
        Status = status;
    }

    public string DisplayName { get; } = "";
    public List<IExpression> Expressions { get; set; } = [];
    public string Reference { get; set; } = "";
    public string Narrative { get; set; } = "";
    public string Conclusion { get; set; } = "";
    public CalcStatus Status { get; set; } = CalcStatus.None;
    public ICalcImage Image { get; set; }


    public static OutputItem New(string narrative)
    {
        return new OutputItem { Narrative = narrative };
    }

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

    public OutputItem AddExpression(IExpression expression)
    {
        Expressions.Add(expression);
        return this;
    }

    public OutputItem AddExpressions(IEnumerable<IExpression> expressions)
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
