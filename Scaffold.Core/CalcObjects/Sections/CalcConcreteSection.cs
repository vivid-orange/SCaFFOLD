using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcConcreteSection : ConcreteSection, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcConcreteSection>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcConcreteSection(IProfile profile, IMaterial material, string name, string symbol = "")
        : base(profile, material)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, string name, string symbol = "")
        : base(profile, material, link)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, Length cover, string name, string symbol = "")
        : base(profile, material, link, cover)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, Length cover, IList<ILongitudinalReinforcement> rebars, string name, string symbol = "")
        : base(profile, material, link, cover, rebars)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcConcreteSection(IProfile profile, IMaterial material, ILink link, string name, string symbol = "")
        : base(profile, material, link)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcConcreteSection(IProfile profile, IMaterial material, ILink link, Length cover, string name, string symbol = "")
        : base(profile, material, link, cover)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcConcreteSection(IProfile profile, IMaterial material, ILink link, Length cover, IList<ILongitudinalReinforcement> rebars, string name, string symbol = "")
        : base(profile, material, link, cover, rebars)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcConcreteSection result)
    {
        try
        {
            result = s.FromJson<CalcConcreteSection>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcConcreteSection Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcConcreteSection>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcConcreteSection result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
