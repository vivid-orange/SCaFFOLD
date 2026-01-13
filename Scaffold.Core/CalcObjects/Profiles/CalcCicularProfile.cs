using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Profiles;

public sealed class CalcCicularProfile : Circle, ICalcProfile<CalcCicularProfile>, ICalcValue, IParsable<CalcCicularProfile>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCicularProfile(Length diameter, string name, string symbol = "")
        : base(diameter)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcCicularProfile(double diameter, LengthUnit unit, string name, string symbol = "")
        : base(new Length(diameter, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCicularProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcCicularProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCicularProfile result)
    {
        try
        {
            result = s.FromJson<CalcCicularProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCicularProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCicularProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCicularProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
