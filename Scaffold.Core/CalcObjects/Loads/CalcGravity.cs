using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcGravity : Gravity, ICalcValue, IParsable<CalcGravity>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcGravity(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcGravity(Ratio z, string name, string symbol = "")
        : base(z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcGravity result)
    {
        try
        {
            result = s.FromJson<CalcGravity>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcGravity Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcGravity>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcGravity result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
