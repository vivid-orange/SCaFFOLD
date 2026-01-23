[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CalcValueTypeAttribute : Attribute
{
    public CalcValueType Type { get; set; }
    public string Symbol { get; set; }
    public string DisplayName { get; set; }

    protected CalcValueTypeAttribute() { }

    public CalcValueTypeAttribute(CalcValueType type) => Type = type;

    /// <summary>
    /// When display name is not set, the property name will be used with spaces added between words (PascalCase to Title Case)
    /// </summary>
    public CalcValueTypeAttribute(CalcValueType type, string symbol, string displayName = null) : this(type)
    {
        Symbol = symbol;
        DisplayName = displayName;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InputCalcValueAttribute : CalcValueTypeAttribute
{
    public InputCalcValueAttribute() : base(CalcValueType.Input) { }
    public InputCalcValueAttribute(string symbol, string displayName = null) : base(CalcValueType.Input, symbol, displayName) { }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class OutputCalcValueAttribute : CalcValueTypeAttribute
{
    public OutputCalcValueAttribute() : base(CalcValueType.Output) { }
    public OutputCalcValueAttribute(string symbol, string displayName = null) : base(CalcValueType.Output, symbol, displayName) { }
}
