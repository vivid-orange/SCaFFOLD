using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Scaffold.Core.Geometry.Abstract;
using Scaffold.Core.Geometry.Enums;

namespace Scaffold.Core.Geometry;

[ExcludeFromCodeCoverage] // because we will be using Kris' libs for this from v1.
public class Line : GeometryBase
{
    Vector2 _start;
    public override Vector2 Start
    {
        get
        {
            return _start;
        }
        set
        {
            _start= value;
        }
    }
    Vector2 _end;
    public override Vector2 End
    {
        get
        {
            return _end;
        }
        set
        {
            _end = value;
        }
    }
    public override double Length
    {
        get
        {
            {
                return (End - Start).Length();
            }
        }
    }

    public override List<GeometryBase> Cut(double parameter)
    {
        if (parameter < 0 || parameter > 1)
        {
            return null;
        }
        else
        {
            Vector2 vector = End - Start;
            return new List<GeometryBase>
            {
                new Line(Start, Start + vector * (float)parameter),
                new Line(Start + vector * (float)parameter, End)
            };
        }
    }

    public Line(Vector2 start, Vector2 end)
    {
        _start = start;
        _end = end;
    }

    public override Vector2 PointAtParameter(double parameter)
    {
        Vector2 vector = End - Start;
        return (float)parameter * vector + Start;
    }

    public override Vector2 PerpAtParameter(double parameter)
    {
        Vector2 v = End - Start;
        var returnVector = new Vector2((
                float)(v.X * Math.Cos(Math.PI / 2) - v.Y * Math.Sin(Math.PI / 2)),
            (float)(v.X * Math.Sin(Math.PI / 2) + v.Y * Math.Cos(Math.PI / 2)));
        return Vector2.Normalize(returnVector);
    }

    public override List<IntersectionResult> intersection(Line line)
    {
        Vector2 p1 = _start;
        Vector2 p1_2 = _end;
        Vector2 p2 = line.Start;
        Vector2 p2_2 = line.End;

        double A1 = p1_2.Y - p1.Y;
        double B1 = p1.X - p1_2.X;
        double C1 = A1 * p1.X + B1 * p1.Y;
        double A2 = p2_2.Y - p2.Y;
        double B2 = p2.X - p2_2.X;
        double C2 = A2 * p2.X + B2 * p2.Y;
        double det = A1 * B2 - A2 * B1;
        double endX = 0; double endY = 0;
        if (det == 0)
        {
            //return new IntersectionResult(IntersectionType.NONE, new Point());
            return new List<IntersectionResult> { new IntersectionResult(double.NaN, new Vector2(), IntersectionType.NONE) };
            //return new List<Vector2>();
        }
        endX = (B2 * C1 - B1 * C2) / det;
        endY = (A1 * C2 - A2 * C1) / det;
        double tol = 0.00000000001;
        if (
            endX + tol >= Math.Min(p1.X, p1_2.X)
            &&
            endX - tol <= Math.Max(p1.X, p1_2.X)
            &&
            endY + tol >= Math.Min(p1.Y, p1_2.Y)
            &&
            endY - tol <= Math.Max(p1.Y, p1_2.Y)
            &&
            endX + tol >= Math.Min(p2.X, p2_2.X)
            &&
            endX - tol <= Math.Max(p2.X, p2_2.X)
            &&
            endY + tol >= Math.Min(p2.Y, p2_2.Y)
            &&
            endY - tol <= Math.Max(p2.Y, p2_2.Y)
        )
        {
            //return new IntersectionResult(IntersectionType.WITHIN, new Point(endX, endY));
            var inter = new Vector2((float)endX, (float)endY);
            double param = (inter - Start).Length() / Length;
            return new List<IntersectionResult> { new IntersectionResult(param, inter, IntersectionType.WITHIN) };
            //return new List<Vector2> { new Vector2((float)endX, (float)endY) };
        }
        else
        {
            //return new IntersectionResult(IntersectionType.PROJECTED, new Point(endX, endY));
            var inter = new Vector2((float)endX, (float)endY);
            double param = (inter - Start).Length() / Length;
            return new List<IntersectionResult> { new IntersectionResult(double.NaN, inter, IntersectionType.PROJECTED) };
            //return new List<Vector2>();

        }
    }
}
