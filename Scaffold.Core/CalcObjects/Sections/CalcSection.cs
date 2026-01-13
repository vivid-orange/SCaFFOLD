using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Sections;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcSection : Section, ICalcValue, IParsable<CalcSection>
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

    public string ValueAsString() => this.ToJson();

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
