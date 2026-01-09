using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcSeismicCombination : SeismicCombination, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcSeismicCombination>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcSeismicCombination(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcSeismicCombination result)
    {
        try
        {
            result = s.FromJson<CalcSeismicCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcSeismicCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcSeismicCombination>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcSeismicCombination result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
