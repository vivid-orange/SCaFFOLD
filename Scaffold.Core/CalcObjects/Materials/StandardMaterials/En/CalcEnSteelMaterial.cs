using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using VividOrange.Taxonomy.Serialization;
using VividOrange.Taxonomy.Standards.Eurocode;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
public sealed class CalcEnSteelMaterial : EnSteelMaterial, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcEnSteelMaterial>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcEnSteelMaterial(EnSteelGrade grade, NationalAnnex nationalAnnex, string name, string symbol = "")
        : base(grade, nationalAnnex)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEnSteelMaterial result)
    {
        try
        {
            result = s.FromJson<CalcEnSteelMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEnSteelMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEnSteelMaterial>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcEnSteelMaterial result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
