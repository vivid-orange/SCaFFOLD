using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Sections;
using VividOrange.Taxonomy.Sections.Reinforcement;
using SkiaSharp;

namespace Scaffold.Core.Images.Drawing
{
    public static class Sections
    {
        private static readonly LengthUnit Unit = LengthUnit.Millimeter;

        public static ICalcImage DrawSection(this ISection section)
        {
            if (section is IConcreteSection concreteSection)
            {
                return concreteSection.DrawConcreteSection();
            }

            var path = new SKPath();
            path.AddProfile(section.Profile);

            (SKColor stroke, SKColor fill) = GetMaterialColours(section.Material);
            var styledPath = new StyledPath(path, stroke, fill);
            return styledPath.DrawFilledPath();
        }

        public static ICalcImage DrawConcreteSection(this IConcreteSection section)
        {
            var path = new SKPath();
            path.AddProfile(section.Profile);
            (SKColor stroke, SKColor fill) = GetMaterialColours(section.Material);
            var edge = new StyledPath(path, stroke, fill);
            var internals = new List<StyledPath>();

            if (section.Link != null)
            {
                var perimeter = new Perimeter(section.Profile);
                var outerPath = new SKPath();
                outerPath.AddPath(perimeter.OuterEdge.Offset(-section.Cover));
                internals.Add(new StyledPath(outerPath, stroke)
                {
                    Fillet = section.Link.Diameter.Millimeters + section.Link.MinimumMandrelDiameter.Millimeters / 2
                });
                var innerPath = new SKPath();
                innerPath.AddPath(perimeter.OuterEdge.Offset(-section.Cover - section.Link.Diameter));
                internals.Add(new StyledPath(innerPath, stroke)
                {
                    Fillet = section.Link.MinimumMandrelDiameter.Millimeters / 2
                });
            }

            if (section.Rebars != null && section.Rebars.Count > 0)
            {
                var rebarPath = new SKPath();
                foreach (ILongitudinalReinforcement rebar in section.Rebars)
                {
                    rebarPath.AddCircle((float)rebar.Position.Y.As(Unit), (float)rebar.Position.Z.As(Unit),
                        (float)rebar.Rebar.Diameter.As(Unit) / 2);
                }

                internals.Add(new StyledPath(rebarPath, stroke, Colours.RebarFill));
            }

            return StyledPath.DrawFilledPaths(edge, internals);
        }

        private static (SKColor Stroke, SKColor Fill) GetMaterialColours(IMaterial material)
        {
            switch (material.Type)
            {
                case MaterialType.Concrete:
                    return (Colours.ConcreteOutline, Colours.ConcreteFill);

                case MaterialType.Steel:
                    return (Colours.SteelOutline, Colours.SteelFill);

                default:
                    return (Colours.BlueDark, Colours.BlueLight);
            }
        }
    }
}
