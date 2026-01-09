using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCustomIProfile : CustomI, ICalcProfile<CalcCustomIProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCustomIProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCustomIProfile(Length height, Length topFlangeWidth, Length bottomFlangeWidth, Length topFlangeThickness, Length bottomFlangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, topFlangeWidth, bottomFlangeWidth, topFlangeThickness, bottomFlangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcCustomIProfile(double height, double topFlangeWidth, double bottomFlangeWidth, double topFlangeThickness, double bottomFlangeThickness, double webThickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(topFlangeWidth, unit), new Length(bottomFlangeWidth, unit), new Length(topFlangeThickness, unit), new Length(bottomFlangeThickness, unit), new Length(webThickness, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCustomIProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcCustomIProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCustomIProfile result)
    {
        try
        {
            result = s.FromJson<CalcCustomIProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCustomIProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCustomIProfile>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcCustomIProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
