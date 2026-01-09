using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcReinforcementLayoutBySpacing : ReinforcementLayoutBySpacing, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcReinforcementLayoutBySpacing>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcReinforcementLayoutBySpacing(IRebar rebar, Length maxSpacing, string name, string symbol = "")
        : base(rebar, maxSpacing)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcReinforcementLayoutBySpacing result)
    {
        try
        {
            result = s.FromJson<CalcReinforcementLayoutBySpacing>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcReinforcementLayoutBySpacing Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcReinforcementLayoutBySpacing>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcReinforcementLayoutBySpacing result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
