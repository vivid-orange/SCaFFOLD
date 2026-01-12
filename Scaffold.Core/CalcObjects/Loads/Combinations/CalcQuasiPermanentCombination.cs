using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads.Combinations;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcQuasiPermanentCombination : QuasiPermanentCombination, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcQuasiPermanentCombination>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcQuasiPermanentCombination(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcQuasiPermanentCombination result)
    {
        try
        {
            result = s.FromJson<CalcQuasiPermanentCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcQuasiPermanentCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcQuasiPermanentCombination>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcQuasiPermanentCombination result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
