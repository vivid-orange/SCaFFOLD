using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcChannelProfile : Channel, ICalcProfile<CalcChannelProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcChannelProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcChannelProfile(Length height, Length width, Length webThickness, Length flangeThickness, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcChannelProfile(double height, double width, double webThickness, double flangeThickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(width, unit), new Length(webThickness, unit), new Length(flangeThickness, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcChannelProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcChannelProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcChannelProfile result)
    {
        try
        {
            result = s.FromJson<CalcChannelProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcChannelProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcChannelProfile>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcChannelProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
