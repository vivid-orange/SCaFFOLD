using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcAccidentalCombination : AccidentalCombination, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcAccidentalCombination>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcAccidentalCombination(string name, string symbol = "")
        : base()
    {
        TypeName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcAccidentalCombination result)
    {
        try
        {
            result = s.FromJson<CalcAccidentalCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcAccidentalCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcAccidentalCombination>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcAccidentalCombination result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
