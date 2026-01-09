using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCProfile : C, ICalcProfile<CalcCProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCProfile(Length height, Length width, Length webThickness, Length flangeThickness, Length lip, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness, lip)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcCProfile(double height, double width, double webThickness, double flangeThickness, double lip, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(width, unit), new Length(webThickness, unit), new Length(flangeThickness, unit), new Length(lip, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcCProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCProfile result)
    {
        try
        {
            result = s.FromJson<CalcCProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCProfile>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcCProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
