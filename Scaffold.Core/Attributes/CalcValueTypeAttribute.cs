using System;

namespace Scaffold.Core.Attributes
{
    public enum CalcValueType
    {
        Input,
        Output
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CalcValueTypeAttribute : Attribute
    {
        public CalcValueType Type { get; set; }
        public string Symbol { get; set; }
        public string DisplayName { get; set; }
        public string[] Headings { get; set; }

        protected CalcValueTypeAttribute() { }

        public CalcValueTypeAttribute(CalcValueType type) => Type = type;

        public CalcValueTypeAttribute(CalcValueType type, string symbol, string displayName = null, params string[] headings) : this(type)
        {
            Symbol = symbol;
            DisplayName = displayName;
            Headings = headings;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InputCalcValueAttribute : CalcValueTypeAttribute
    {
        public InputCalcValueAttribute() : base(CalcValueType.Input) { }
        public InputCalcValueAttribute(string symbol, string displayName = null, params string[] headings)
            : base(CalcValueType.Input, symbol, displayName, headings) { }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class OutputCalcValueAttribute : CalcValueTypeAttribute
    {
        public OutputCalcValueAttribute() : base(CalcValueType.Output) { }
        public OutputCalcValueAttribute(string symbol, string displayName = null, params string[] headings)
            : base(CalcValueType.Output, symbol, displayName, headings) { }
    }
}
