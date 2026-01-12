using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Scaffold.Core.Geometry.Abstract;
using Scaffold.Core.Geometry.Enums;

namespace Scaffold.Core.Geometry;

[ExcludeFromCodeCoverage] // because we will be using Kris' libs for this from v1.
public class Arc : GeometryBase
{
    public Vector2 Centre { get; set; }
    double startAngle;
    public double StartAngle
    {
        get
        {
            return startAngle;
        }
        set
        {
            startAngle = value;
            checkAngleRange();
        }
    }

    private void checkAngleRange()
    {
        if (startAngle > Math.PI * 2 && endAngle > Math.PI * 2)
        {
            startAngle -= 2 * Math.PI;
            endAngle -= 2 * Math.PI;
        }
        ;
    }

    double endAngle;
    public double EndAngle
    {
        get
        {
            return endAngle;
        }
        set
        {
            endAngle = value;
            checkAngleRange();
        }
    }
    double _radius;
    public double Radius
    {
        get
        {
            return _radius;
        }
        set
        {
            _radius = Math.Abs(value);
        }
    }
    public override Vector2 Start
    {
        get
        {
            return new Vector2((float)(Centre.X + Radius * Math.Cos(StartAngle)), (float)(Centre.Y + Radius * Math.Sin(StartAngle)));
        }
    }
    public override Vector2 End
    {
        get
        {
            return new Vector2((float)(Centre.X + Radius * Math.Cos(EndAngle)), (float)(Centre.Y + Radius * Math.Sin(EndAngle)));
        }
    }
    public override double Length
    {
        get
        {
            if (StartAngle == EndAngle)
            {
                return 0;
            }
            if (StartAngle > EndAngle)
            {
                double end = EndAngle + Math.PI;
                return ((end - StartAngle) / (2 * Math.PI)) * 2 * Math.PI * Radius;
            }
            else
            {
                double end = EndAngle;
                return ((end - StartAngle) / (2 * Math.PI)) * 2 * Math.PI * Radius;
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
            double diffAngle = parameter * (EndAngle - StartAngle);
            if (diffAngle < 0)
            {
                diffAngle += 2 * Math.PI;
            }

            return new List<GeometryBase>
            {
                new Arc{Centre = Centre, StartAngle = StartAngle, EndAngle = StartAngle + diffAngle, Radius = Radius},
                new Arc{Centre = Centre, StartAngle = StartAngle + diffAngle, EndAngle = EndAngle, Radius = Radius}
            };
        }
    }

    public override Vector2 PointAtParameter(double parameter)
    {
        double diffAngle = parameter * (EndAngle - StartAngle);
        if (diffAngle < 0)
        {
            diffAngle += 2 * Math.PI;
        }
        return new Vector2((float)(Centre.X + Radius * Math.Cos(StartAngle + diffAngle)), (float)(Centre.Y + Radius * Math.Sin(StartAngle + diffAngle)));
    }

    public override Vector2 PerpAtParameter(double parameter)
    {
        double diffAngle = parameter * (EndAngle - StartAngle);
        if (diffAngle < 0)
        {
            diffAngle += 2 * Math.PI;
        }
        var point = new Vector2((float)(Centre.X + Radius * Math.Cos(StartAngle + diffAngle)), (float)(Centre.Y + Radius * Math.Sin(StartAngle + diffAngle)));
        Vector2 returnVector = point - Centre;
        return Vector2.Normalize(returnVector);
    }

    // Find the points of intersection.
    public override List<IntersectionResult> intersection(Line line)
    {
        Arc arc = this;
        Vector2 point1 = line.Start;
        Vector2 point2 = line.End;
        float cx = arc.Centre.X;
        float cy = arc.Centre.Y;
        float radius = (float)arc.Radius;

        Vector2 intersection1;
        Vector2 intersection2;

        float dx, dy, A, B, C, det, t;

        dx = point2.X - point1.X;
        dy = point2.Y - point1.Y;

        A = dx * dx + dy * dy;
        B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
        C = (point1.X - cx) * (point1.X - cx) + (point1.Y - cy) * (point1.Y - cy) - radius * radius;

        det = B * B - 4 * A * C;
        if ((A <= 0.0000001) || (det < 0))
        {
            // No real solutions.
            return new List<IntersectionResult>();
        }
        else if (det == 0)
        {
            // One solution.
            var returnList = new List<IntersectionResult>();
            t = -B / (2 * A);
            intersection1 = new Vector2(point1.X + t * dx, point1.Y + t * dy);
            double angle = Math.Atan2(intersection1.Y - arc.Centre.Y, intersection1.X - arc.Centre.X);
            if (angle < 0)
            {
                angle += 2 * Math.PI;
            }
            float endX = intersection1.X; float endY = intersection1.Y; float tol = 0.000001f;
            if (angle >= arc.StartAngle &&
                angle <= arc.EndAngle &&
                endX + tol >= Math.Min(point1.X, point2.X) &&
                endX - tol <= Math.Max(point1.X, point2.X) &&
                endY + tol >= Math.Min(point1.Y, point2.Y) &&
                endY - tol <= Math.Max(point1.Y, point2.Y))
            {
                double temp = angle - StartAngle;
                if (temp < 0) temp += 2 * Math.PI;

                double param = (Radius * temp) / (Length);

                returnList.Add(new IntersectionResult(param, intersection1, IntersectionType.WITHIN));
            }
            return returnList;
        }
        else
        {
            // Two solutions.
            var returnList = new List<IntersectionResult>();
            t = (float)((-B + Math.Sqrt(det)) / (2 * A));
            intersection1 = new Vector2(point1.X + t * dx, point1.Y + t * dy);
            double angle = Math.Atan2(intersection1.Y - arc.Centre.Y, intersection1.X - arc.Centre.X);
            if (angle < 0)
            {
                angle += 2 * Math.PI;
            }
            float endX = intersection1.X; float endY = intersection1.Y; float tol = 0.000001f;
            if (angle >= arc.StartAngle &&
                angle <= arc.EndAngle &&
                endX + tol >= Math.Min(point1.X, point2.X) &&
                endX - tol <= Math.Max(point1.X, point2.X) &&
                endY + tol >= Math.Min(point1.Y, point2.Y) &&
                endY - tol <= Math.Max(point1.Y, point2.Y))
            {
                double temp = angle - StartAngle;
                if (temp < 0) temp += 2 * Math.PI;

                double param = (Radius * temp) / (Length);

                returnList.Add(new IntersectionResult(param, intersection1, IntersectionType.WITHIN));
            }
            t = (float)((-B - Math.Sqrt(det)) / (2 * A));
            intersection2 = new Vector2(point1.X + t * dx, point1.Y + t * dy);
            angle = Math.Atan2(intersection2.Y - arc.Centre.Y, intersection2.X - arc.Centre.X);
            if (angle < 0)
            {
                angle += 2 * Math.PI;
            }
            endX = intersection2.X; endY = intersection2.Y;
            if (angle >= arc.StartAngle &&
                angle <= arc.EndAngle &&
                endX + tol >= Math.Min(point1.X, point2.X) &&
                endX - tol <= Math.Max(point1.X, point2.X) &&
                endY + tol >= Math.Min(point1.Y, point2.Y) &&
                endY - tol <= Math.Max(point1.Y, point2.Y))
            {
                double temp = angle - StartAngle;
                if (temp < 0) temp += 2 * Math.PI;

                double param = (Radius * temp) / (Length);

                returnList.Add(new IntersectionResult(param, intersection2, IntersectionType.WITHIN));
            }
            return returnList;
        }
    }
}
