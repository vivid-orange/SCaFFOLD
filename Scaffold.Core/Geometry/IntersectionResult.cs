using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Scaffold.Geometry;

[ExcludeFromCodeCoverage] // because we will be using Kris' libs for this from v1.
public class IntersectionResult
{
    public double Parameter { get; private set; }
    public Vector2 Point { get; private set; }
    public IntersectionType TypeOfIntersection { get; private set; }

    public IntersectionResult(double parameter, Vector2 point, IntersectionType interType)
    {
        Parameter = parameter;
        Point = point;
        TypeOfIntersection = interType;
    }

    public override string ToString()
    {
        return string.Format("Point {0}; parameter {1}; type {2}", Point, Parameter, TypeOfIntersection);
    }
}
