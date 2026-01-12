using VividOrange.Taxonomy.Sections.Reinforcement;
using VividOrange.Taxonomy.Serialization;
using VividOrange.Taxonomy.Standards.Eurocode;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcMinimumReinforcementSpacing : MinimumReinforcementSpacing, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcMinimumReinforcementSpacing>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcMinimumReinforcementSpacing(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcMinimumReinforcementSpacing(NationalAnnex nationalAnnex, string name, string symbol = "")
        : base(nationalAnnex)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcMinimumReinforcementSpacing result)
    {
        try
        {
            result = s.FromJson<CalcMinimumReinforcementSpacing>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcMinimumReinforcementSpacing Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcMinimumReinforcementSpacing>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcMinimumReinforcementSpacing result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
