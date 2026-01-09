using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcRebar : Rebar, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcRebar>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRebar(IMaterial material, Length diameter, string name, string symbol = "")
        : base(material, diameter)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcRebar(IMaterial material, BarDiameter diameter, string name, string symbol = "")
        : base(material, diameter)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRebar result)
    {
        try
        {
            result = s.FromJson<CalcRebar>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRebar Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRebar>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRebar result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
