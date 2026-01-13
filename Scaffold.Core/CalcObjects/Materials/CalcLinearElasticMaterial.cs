using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Materials;

public sealed class CalcLinearElasticMaterial : LinearElasticMaterial, ICalcValue, IParsable<CalcLinearElasticMaterial>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcLinearElasticMaterial(MaterialType type, Pressure elasticModulus, Pressure strength, string name, string symbol = "")
        : base(type, elasticModulus, strength)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLinearElasticMaterial result)
    {
        try
        {
            result = s.FromJson<CalcLinearElasticMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLinearElasticMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLinearElasticMaterial>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcLinearElasticMaterial result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
