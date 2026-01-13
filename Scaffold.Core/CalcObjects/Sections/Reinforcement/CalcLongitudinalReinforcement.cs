using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Geometry;
using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Sections.Reinforcement;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcLongitudinalReinforcement : LongitudinalReinforcement, ICalcValue, IParsable<CalcLongitudinalReinforcement>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcLongitudinalReinforcement(IRebar rebar, ILocalPoint2d position, string name, string symbol = "")
        : base(rebar, position)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcLongitudinalReinforcement(IMaterial material, Length diameter, ILocalPoint2d position, string name, string symbol = "")
        : base(material, diameter, position)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLongitudinalReinforcement result)
    {
        try
        {
            result = s.FromJson<CalcLongitudinalReinforcement>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLongitudinalReinforcement Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLongitudinalReinforcement>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcLongitudinalReinforcement result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
