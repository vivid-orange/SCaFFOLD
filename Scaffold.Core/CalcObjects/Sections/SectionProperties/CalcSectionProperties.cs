using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Sections;
using VividOrange.Taxonomy.Sections.SectionProperties;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcSectionProperties : SectionProperties, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcSectionProperties>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcSectionProperties(ISection section, string name, string symbol = "")
        : base(section)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcSectionProperties(IProfile profile, string name, string symbol = "")
        : base(profile)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcSectionProperties result)
    {
        try
        {
            result = s.FromJson<CalcSectionProperties>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcSectionProperties Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcSectionProperties>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcSectionProperties result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
