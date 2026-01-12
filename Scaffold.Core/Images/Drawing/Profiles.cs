using VividOrange.Taxonomy.Profiles;
using SkiaSharp;

namespace Scaffold.Core.Images.Drawing
{
    public static class Profiles
    {
        public static ICalcImage DrawProfile(this IProfile profile, SKColor stroke, SKColor fill)
        {
            var path = new SKPath();
            path.AddProfile(profile);
            var styledPath = new StyledPath(path, stroke, fill);
            return styledPath.DrawFilledPath();
        }

        public static void AddProfile(this SKPath path, IProfile profile, LengthUnit unit = LengthUnit.Millimeter)
        {
            var perimeter = new Perimeter(profile);
            path.AddPoly(Geometries.ConvertPoints(perimeter.OuterEdge, unit));
            if (perimeter.VoidEdges != null)
            {
                foreach (var voidEdge in perimeter.VoidEdges)
                {
                    path.AddPoly(Geometries.ConvertPoints(voidEdge, unit));
                }
            }
        }
    }
}
