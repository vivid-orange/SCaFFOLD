using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads.Cases;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads.Cases;
public sealed class CalcVariableCase : VariableCase, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcVariableCase>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcVariableCase(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcVariableCase result)
    {
        try
        {
            result = s.FromJson<CalcVariableCase>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcVariableCase Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcVariableCase>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcVariableCase result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
