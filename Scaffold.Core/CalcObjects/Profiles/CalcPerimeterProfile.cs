using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using VividOrange.Geometry;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcPerimeterProfile : Perimeter, ICalcProfile<CalcPerimeterProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPerimeterProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPerimeterProfile(ILocalPolyline2d edge, string name, string symbol = "")
        : base(edge)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeterProfile(IList<ILocalPoint2d> edgePoints, string name, string symbol = "")
        : base(edgePoints)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeterProfile(ILocalPolyline2d outerEdge, IList<ILocalPolyline2d> voidEdges, string name, string symbol = "")
        : base(outerEdge, voidEdges)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeterProfile(IProfile profile, string name, string symbol = "")
        : base(profile)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeterProfile(IProfile profile, Length tolerance, string name, string symbol = "")
        : base(profile, tolerance)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeterProfile(IProfile profile, int divisions, string name, string symbol = "")
        : base(profile, divisions)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcPerimeterProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcPerimeterProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPerimeterProfile result)
    {
        try
        {
            result = s.FromJson<CalcPerimeterProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPerimeterProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPerimeterProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPerimeterProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
