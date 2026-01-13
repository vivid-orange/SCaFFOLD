using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Profiles;

public sealed class CalcTeeProfile : Tee, ICalcProfile<CalcTeeProfile>, ICalcValue, IParsable<CalcTeeProfile>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcTeeProfile(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, width, flangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcTeeProfile(double height, double width, double flangeThickness, double webThickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(width, unit), new Length(flangeThickness, unit), new Length(webThickness, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcTeeProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcTeeProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcTeeProfile result)
    {
        try
        {
            result = s.FromJson<CalcTeeProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcTeeProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcTeeProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcTeeProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
