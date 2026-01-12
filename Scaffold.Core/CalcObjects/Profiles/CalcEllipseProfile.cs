using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcEllipseProfile : Ellipse, ICalcProfile<CalcEllipseProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcEllipseProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcEllipseProfile(Length width, Length height, string name, string symbol = "")
        : base(width, height)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcEllipseProfile(double width, double height, LengthUnit unit, string name, string symbol = "")
        : base(new Length(width, unit), new Length(height, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcEllipseProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcEllipseProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEllipseProfile result)
    {
        try
        {
            result = s.FromJson<CalcEllipseProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEllipseProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEllipseProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcEllipseProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
