using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcLinearElasticOrthotropicMaterial : LinearElasticOrthotropicMaterial, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcLinearElasticOrthotropicMaterial>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcLinearElasticOrthotropicMaterial(MaterialType type, Pressure elasticModulusX, Pressure strengthX, Pressure elasticModulusY, Pressure strengthY, Pressure elasticModulusZ, Pressure strengthZ, string name, string symbol = "")
        : base(type, elasticModulusX, strengthX, elasticModulusY, strengthY, elasticModulusZ, strengthZ)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLinearElasticOrthotropicMaterial result)
    {
        try
        {
            result = s.FromJson<CalcLinearElasticOrthotropicMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLinearElasticOrthotropicMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLinearElasticOrthotropicMaterial>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcLinearElasticOrthotropicMaterial result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
