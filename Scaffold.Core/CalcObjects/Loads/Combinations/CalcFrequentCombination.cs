using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcFrequentCombination : FrequentCombination, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcFrequentCombination>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcFrequentCombination(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcFrequentCombination result)
    {
        try
        {
            result = s.FromJson<CalcFrequentCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcFrequentCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcFrequentCombination>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcFrequentCombination result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
