using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Sections.Reinforcement;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcReinforcementLayoutByCount : ReinforcementLayoutByCount, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcReinforcementLayoutByCount>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcReinforcementLayoutByCount(IRebar rebar, int numberOfBars, string name, string symbol = "")
        : base(rebar, numberOfBars)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcReinforcementLayoutByCount result)
    {
        try
        {
            result = s.FromJson<CalcReinforcementLayoutByCount>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcReinforcementLayoutByCount Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcReinforcementLayoutByCount>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcReinforcementLayoutByCount result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
