namespace Scaffold;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CalcParameterAttribute : Attribute, ICalcParameter
{
    public CalcParameterType Type { get; internal set; }
    public string Symbol { get; internal set; } = "";
    public string EntityLabel { get; internal set; } = "";
    public string[]? Headings { get; internal set; }

    protected CalcParameterAttribute() { }

    public CalcParameterAttribute(CalcParameterType type) => Type = type;

    /// <summary>
    /// When display name is not set, the property name will be used with spaces added between words (PascalCase to Title Case)
    /// </summary>
    public CalcParameterAttribute(
        CalcParameterType type, string symbol, string displayName = "", string[]? headings = null)
        : this(type)
    {
        Symbol = symbol;
        EntityLabel = displayName;
        Headings = headings;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InputParameterAttribute : CalcParameterAttribute
{
    public InputParameterAttribute() : base(CalcParameterType.Input) { }
    public InputParameterAttribute(string symbol, string displayName = "", string[]? headings = null)
        : base(CalcParameterType.Input, symbol, displayName, headings) { }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class OutputParameterAttribute : CalcParameterAttribute
{
    public OutputParameterAttribute() : base(CalcParameterType.Output) { }
    public OutputParameterAttribute(string symbol, string displayName = "", string[]? headings = null)
        : base(CalcParameterType.Output, symbol, displayName, headings) { }
}
