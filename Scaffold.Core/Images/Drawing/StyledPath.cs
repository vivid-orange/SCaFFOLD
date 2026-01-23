using SkiaSharp;

namespace Scaffold.Drawing;

public class StyledPath
{
    public SKPath Path { get; set; }
    public SKColor Stroke { get; set; }
    public int StrokeWeight { get; set; } = 1;
    public SKColor Fill { get; set; } = SKColor.Empty;
    public double Fillet { get; set; } = 0;

    public StyledPath(SKPath path, SKColor stroke)
    {
        Path = path;
        Stroke = stroke;
    }

    public StyledPath(SKPath path, SKColor stroke, SKColor fill)
    {
        Path = path;
        Stroke = stroke;
        Fill = fill;
    }

    public SKRect GetBounds()
    {
        Path.GetBounds(out SKRect rect);
        return rect;
    }

    public ICalcImage DrawFilledPath(float scalar = 4, int padding = 16)
    {
        SKRect bounds = GetBounds();
        var bitmap = new SKBitmap((int)bounds.Width * (int)scalar + padding, (int)bounds.Height * (int)scalar + padding);
        var canvas = new SKCanvas(bitmap);
        canvas.DrawFilledPath(this, bounds, scalar, padding);
        return new ImageFromSkBitmap(bitmap);
    }

    public static ICalcImage DrawFilledPaths(StyledPath edge, List<StyledPath> internals, float scalar = 4, int padding = 16)
    {
        SKRect bounds = edge.GetBounds();
        var bitmap = new SKBitmap((int)bounds.Width * (int)scalar + padding, (int)bounds.Height * (int)scalar + padding);
        var canvas = new SKCanvas(bitmap);
        canvas.DrawFilledPath(edge, bounds, scalar, padding);
        if (internals != null && internals.Count > 0)
        {
            foreach (StyledPath path in internals)
            {
                canvas.DrawFilledPath(path, bounds, scalar, padding);
            }
        }

        return new ImageFromSkBitmap(bitmap);
    }
}

public static class StyledPathExtensions
{
    public static void DrawFilledPath(this SKCanvas canvas, StyledPath sp, SKRect? bounds = null, float scalar = 4, int padding = 16)
    {
        if (bounds == null || bounds.Value.IsEmpty)
        {
            bounds = sp.GetBounds();
        }

        var pathPositiveCoordinate = new SKPath(sp.Path);
        pathPositiveCoordinate.Transform(SKMatrix.CreateScale(scalar, scalar));
        pathPositiveCoordinate.Offset(((int)bounds.Value.Width * scalar + padding) / 2, ((int)bounds.Value.Height * scalar + padding) / 2);

        SKPaint paint = new SKPaint();
        if (sp.Fill != SKColor.Empty)
        {
            paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = sp.Fill,
            };
            if (sp.Fillet != 0)
            {
                paint.PathEffect = SKPathEffect.CreateCorner((float)sp.Fillet * scalar);
            }

            canvas.DrawPath(pathPositiveCoordinate, paint);
        }

        paint.Style = SKPaintStyle.Stroke;
        paint.Color = sp.Stroke;
        paint.StrokeWidth = sp.StrokeWeight * scalar;
        if (sp.Fillet != 0)
        {
            paint.PathEffect = SKPathEffect.CreateCorner((float)sp.Fillet * scalar);
        }

        canvas.DrawPath(pathPositiveCoordinate, paint);
    }
}
