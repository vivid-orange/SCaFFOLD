using SkiaSharp;
using VividOrange.Geometry;

namespace Scaffold.Core.Images.Drawing
{
    public static class Geometries
    {
        public static SKPath ConvertToPath(this ILocalPolyline2d polygon, LengthUnit unit = LengthUnit.Millimeter)
        {
            var path = new SKPath();
            path.AddPoly(ConvertPoints(polygon, unit));
            return path;
        }

        public static SKPath AddPath(this SKPath path, ILocalPolyline2d polygon, LengthUnit unit = LengthUnit.Millimeter)
        {
            path.AddPoly(ConvertPoints(polygon, unit));
            return path;
        }

        public static SKPath ConvertToPath(this IPolyline2d polygon, LengthUnit unit = LengthUnit.Millimeter)
        {
            var path = new SKPath();
            path.AddPoly(ConvertPoints(polygon, unit));
            return path;
        }

        internal static SKPoint[] ConvertPoints(ILocalPolyline2d polyline, LengthUnit unit = LengthUnit.Millimeter)
        {
            var pts = new SKPoint[polyline.Points.Count];
            int i = 0;
            foreach (ILocalPoint2d point in polyline.Points)
            {
                pts[i++] = new SKPoint((float)point.Y.As(unit), (float)point.Z.As(unit));
            }

            return pts;
        }

        private static SKPoint[] ConvertPoints(IPolyline2d polyline, LengthUnit unit = LengthUnit.Millimeter)
        {
            var pts = new SKPoint[polyline.Points.Count];
            int i = 0;
            foreach (IPoint2d point in polyline.Points)
            {
                pts[i++] = new SKPoint((float)point.U.As(unit), (float)point.V.As(unit));
            }

            return pts;
        }
    }
}
