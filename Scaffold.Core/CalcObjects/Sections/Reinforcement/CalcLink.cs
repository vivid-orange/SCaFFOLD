using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Sections.Reinforcement;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;

public sealed class CalcLink : Link, ICalcValue, IParsable<CalcLink>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcLink(IMaterial material, Length diameter, string name, string symbol = "")
        : base(material, diameter)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcLink(IMaterial material, BarDiameter diameter, string name, string symbol = "")
        : base(material, diameter)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcLink(IRebar rebar, string name, string symbol = "")
        : base(rebar)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLink result)
    {
        try
        {
            result = s.FromJson<CalcLink>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLink Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLink>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcLink result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
