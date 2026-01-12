using VividOrange.Taxonomy.Loads.Cases;
using VividOrange.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads.Cases;
public sealed class CalcPermanentCase : PermanentCase, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPermanentCase>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPermanentCase(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPermanentCase result)
    {
        try
        {
            result = s.FromJson<CalcPermanentCase>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPermanentCase Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPermanentCase>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPermanentCase result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
