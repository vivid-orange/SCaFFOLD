using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Scaffold.Core.Geometry.Abstract;

[ExcludeFromCodeCoverage] // because we will be using Kris' libs for this from v1.
public abstract class GeometryBase
{
    public abstract Vector2 Start { get; set; }
    public abstract Vector2 End { get; set; }
    public abstract double Length { get; }
    public abstract List<IntersectionResult> intersection(Line line);
    public abstract List<GeometryBase> Cut(double parameter);
    public abstract Vector2 PointAtParameter(double parameter);
    public abstract Vector2 PerpAtParameter(double parameter);
}
