using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Numerics;
using Scaffold.Core.Geometry.Abstract;
using Scaffold.Core.Geometry.Enums;

namespace Scaffold.Core.Geometry;

[ExcludeFromCodeCoverage] // because we will be using Kris' libs for this from v1.
public class PolyLine : GeometryBase
{
    List<GeometryBase> _segments;
    public List<GeometryBase> Segments
    {
        get
        {
            return _segments.ToList();
        }
    }


    public override Vector2 Start
    {
        get
        {
            return _segments.First().Start;
        }
        set { } //TODO
    }

    public override Vector2 End
    {
        get
        {
            return _segments.Last().End;
        }
        set { } //TODO
    }

    public override double Length
    {
        get
        {
            double returnLength = 0;
            foreach (GeometryBase item in _segments)
            {
                returnLength += item.Length;
            }
            return returnLength;
        }
    }

    bool _isClosed = false;
    public bool IsClosed
    {
        get
        {
            return _isClosed;
        }
        set
        {
            if (value)
            {
                if ((End - Start).Length() < 0.1)
                {
                    _isClosed = value;
                }
                else
                {
                    _segments.Add(new Line(End, Start));
                }
            }
            else
                _isClosed = value;
        }
    }

    public PolyLine(List<GeometryBase> segments)
    {
        _segments = segments;
    }

    public override List<IntersectionResult> intersection(Line line) // need to add full support for NONE and PROJECTED 
    {
        double totalLength = Length;
        double lengthToSegment = 0;
        var returnList = new List<IntersectionResult>();
        foreach (GeometryBase segment in _segments)
        {
            List<IntersectionResult> intersections = segment.intersection(line);
            foreach (IntersectionResult intersection in intersections)
            {
                if (intersection.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    returnList.Add(new IntersectionResult((lengthToSegment + intersection.Parameter * segment.Length) / totalLength, intersection.Point, intersection.TypeOfIntersection));
                }
            }
            lengthToSegment += segment.Length;
        }
        if (returnList.Count == 0)
        {
            returnList.Add(new IntersectionResult(double.NaN, new Vector2(), IntersectionType.NONE));
        }
        return returnList;
    }

    public override string ToString()
    {
        string returnString = "";
        returnString = "Start " + _segments[0].Start.ToString();
        returnString += Environment.NewLine;

        foreach (GeometryBase item in _segments)
        {
            if (item.GetType() == typeof(Arc))
            {
                returnString += " Arc ";
                returnString += item.End;
                returnString += Environment.NewLine;
            }
            if (item.GetType() == typeof(Line))
            {
                returnString += " Line ";
                returnString += item.End;
                returnString += Environment.NewLine;
            }
        }

        return returnString;
    }

    public override Vector2 PointAtParameter(double parameter)
    {
        double totalLength = Length;
        double lengthToSegmentStart = 0;
        double lengthToSegmentEnd = 0;
        for (int i = 0; i < _segments.Count; i++)
        {
            GeometryBase segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            double parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if (parameter >= parameterToSegmentStart && parameter <= parameterToSegmentEnd)
            {
                double segmentParameter = (parameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                return segment.PointAtParameter(segmentParameter);
            }
        }
        return _segments.Last().PointAtParameter(1);
    }

    public override  Vector2 PerpAtParameter(double parameter)
    {
        double totalLength = Length;
        double lengthToSegmentStart = 0;
        double lengthToSegmentEnd = 0;
        for (int i = 0; i < _segments.Count; i++)
        {
            GeometryBase segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            double parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if (parameter >= parameterToSegmentStart && parameter <= parameterToSegmentEnd)
            {
                double segmentParameter = (parameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                return segment.PerpAtParameter(segmentParameter);
            }
        }
        return _segments.Last().PerpAtParameter(1);
    }

    public PolyLine Cut(double startParameter, double endParameter)
    {
        double totalLength = Length;
        double lengthToSegmentStart = 0;
        double lengthToSegmentEnd = 0;
        bool started = false;
        bool finished = false;
        var segments = new List<GeometryBase>();
        for (int i = 0; i < _segments.Count; i++)
        {
            GeometryBase segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            double parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if ((endParameter > parameterToSegmentEnd && started) || (endParameter <= startParameter && started))
            {
                segments.Add(segment);
            }
            else if (startParameter <= parameterToSegmentEnd && !started)
            {
                if (endParameter == 0.1)
                {
                    //
                }
                if (endParameter <= parameterToSegmentEnd && endParameter >= startParameter)
                {
                    double segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                    if (double.IsNaN(segmentParameter))
                        segmentParameter = 0;
                    GeometryBase partialCut = segment.Cut(segmentParameter)[0];
                    double secondSegmentParameter = (startParameter - parameterToSegmentStart) / (endParameter - parameterToSegmentStart);
                    segments.Add(partialCut.Cut(secondSegmentParameter)[1]);
                    started = true;
                    finished = true;
                }
                else
                {
                    double segmentParameter = (startParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                    if (double.IsNaN(segmentParameter))
                        segmentParameter = 0;
                    segments.Add(segment.Cut(segmentParameter)[1]);
                    started = true;
                }
            }
            else if (endParameter <= parameterToSegmentEnd && endParameter >= startParameter && started && !finished)
            {
                double segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                segments.Add(segment.Cut(segmentParameter)[0]);
                finished = true;
                break;
            }
        }
        if (finished)
        {
            return new PolyLine(segments);
        }
        lengthToSegmentEnd = 0;
        lengthToSegmentStart = 0;
        for (int i = 0; i < _segments.Count; i++)
        {
            GeometryBase segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            double parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if (endParameter > parameterToSegmentEnd)
            {
                segments.Add(segment);
            }
            //else if (startParameter <= parameterToSegmentEnd && !started)
            //{
            //    double segmentParameter = (startParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
            //    segments.Add(segment.Cut(segmentParameter)[1]);
            //    started = true;
            //}
            else if (endParameter <= parameterToSegmentEnd)
            {
                double segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                segments.Add(segment.Cut(segmentParameter)[0]);
                finished = true;
                break;
            }
        }
        return new PolyLine(segments);
    }

    public override List<GeometryBase> Cut(double parameter)
    {
        var returnList = new List<GeometryBase>();

        returnList.Add(Cut(0, parameter));
        returnList.Add(Cut(parameter, 0));

        return returnList;
    }
}
