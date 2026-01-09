using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.SectionProperties;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcConcreteSectionProperties : ConcreteSectionProperties, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcConcreteSectionProperties>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcConcreteSectionProperties(IConcreteSection section, string name, string symbol = "")
        : base(section)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcConcreteSectionProperties result)
    {
        try
        {
            result = s.FromJson<CalcConcreteSectionProperties>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcConcreteSectionProperties Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcConcreteSectionProperties>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcConcreteSectionProperties result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
