using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcSection : Section, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcSection>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcSection(IProfile profile, IMaterial material, string name, string symbol = "")
        : base(profile, material)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcSection result)
    {
        try
        {
            result = s.FromJson<CalcSection>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcSection Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcSection>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcSection result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
