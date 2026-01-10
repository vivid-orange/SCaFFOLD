using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcLink : Link, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcLink>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcLink(IMaterial material, Length diameter, string name, string symbol = "")
        : base(material, diameter)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcLink(IMaterial material, BarDiameter diameter, string name, string symbol = "")
        : base(material, diameter)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcLink(IRebar rebar, string name, string symbol = "")
        : base(rebar)
    {
        TypeName = name;
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

    public string GetValueAsString() => this.ToJson();

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
