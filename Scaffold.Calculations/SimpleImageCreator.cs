using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Calculations
{
    public class Utilities
    {
        public static SKBitmap CreateSampleImage(float circleDiameter, SKColor circleColor)
        {
            // 1. Define image dimensions (Fixed at 100x100 per original request)
            int width = 100;
            int height = 100;
            var imageInfo = new SKImageInfo(width, height);

            var bitmap = new SKBitmap(imageInfo);

            using (var canvas = new SKCanvas(bitmap))
            {
                // 2. Draw the fixed Light Grey Background
                canvas.Clear(SKColors.LightGray);

                using (var paint = new SKPaint())
                {
                    // 3. Apply the custom color from the parameter
                    paint.Color = circleColor;
                    paint.IsAntialias = true;
                    paint.Style = SKPaintStyle.Fill;

                    // 4. Calculate position and radius
                    float centerX = width / 2f;
                    float centerY = height / 2f;

                    // Calculate radius from the input diameter
                    float radius = circleDiameter / 2f;

                    // 5. Draw the circle
                    canvas.DrawCircle(centerX, centerY, radius, paint);
                }
            }

            return bitmap;
        }

        public static SKBitmap CreateMultiCircleImage(List<double[]> circlesData, SKColor circleColor)
        {
            // 1. Define image dimensions (Fixed at 100x100)
            int width = 100;
            int height = 100;
            var imageInfo = new SKImageInfo(width, height);

            var bitmap = new SKBitmap(imageInfo);

            using (var canvas = new SKCanvas(bitmap))
            {
                // 2. Background: Light Grey
                canvas.Clear(SKColors.LightGray);

                using (var paint = new SKPaint())
                {
                    paint.Color = circleColor; // Default color for all circles
                    paint.IsAntialias = true;
                    paint.Style = SKPaintStyle.Fill;

                    // 3. Iterate through the input list
                    foreach (var data in circlesData)
                    {
                        // Ensure the array has at least 3 elements to avoid errors
                        if (data != null && data.Length >= 3)
                        {
                            // Extract values (converting double to float for SkiaSharp)
                            float x = (float)data[0];
                            float y = (float)data[1];
                            float diameter = (float)data[2];

                            float radius = diameter / 2f;

                            // Draw this specific circle
                            canvas.DrawCircle(x, y, radius, paint);
                        }
                    }
                }
            }

            return bitmap;
        }

        public static SKBitmap CreateDetailedISectionBitmap(
            double height,
            double breadth,
            double flangeThickness,
            double webThickness,
            double rootRadius,
            SKColor sectionColor)
        {
            // 1. Define Padding
            float padding = 10f;

            // 2. Cast doubles to floats for SkiaSharp
            float b = (float)breadth;
            float h = (float)height;
            float ft = (float)flangeThickness;
            float wt = (float)webThickness;
            float r = (float)rootRadius;

            // 3. Calculate Canvas Dimensions
            int canvasWidth = (int)Math.Ceiling(b) + (int)(padding * 2);
            int canvasHeight = (int)Math.Ceiling(h) + (int)(padding * 2);

            SKBitmap bitmap = new SKBitmap(canvasWidth, canvasHeight);

            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // 4. Set Background
                canvas.Clear(SKColors.LightGray);

                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = sectionColor;
                    paint.Style = SKPaintStyle.Fill;
                    paint.IsAntialias = true;

                    // 5. Calculate Key Coordinates
                    // Determine where the web starts (X-axis)
                    float webLeft = padding + (b - wt) / 2;
                    float webRight = webLeft + wt;

                    // Y-axis boundaries for flanges
                    float topFlangeBottom = padding + ft;
                    float bottomFlangeTop = padding + h - ft;

                    // 6. Construct the Path
                    // We draw the perimeter clockwise starting from Top-Left
                    SKPath path = new SKPath();

                    // A. Top Flange (Top edge)
                    path.MoveTo(padding, padding);                             // Top-Left
                    path.LineTo(padding + b, padding);                         // Top-Right

                    // B. Top Flange (Right edge)
                    path.LineTo(padding + b, topFlangeBottom);                 // Bottom-Right of top flange

                    // C. Top-Right Fillet (Transition to Web)
                    // Move left along underside of flange to the start of the fillet
                    path.LineTo(webRight + r, topFlangeBottom);
                    // Arc down-left into the web
                    // ArcTo parameters: radiusX, radiusY, xAxisRotate, size, direction, destX, destY
                    path.ArcTo(r, r, 0, SKPathArcSize.Small, SKPathDirection.CounterClockwise, webRight, topFlangeBottom + r);

                    // D. Web (Right edge)
                    // Go down the web to the start of the bottom fillet
                    path.LineTo(webRight, bottomFlangeTop - r);

                    // E. Bottom-Right Fillet (Transition to Flange)
                    // Arc out-right to the bottom flange
                    path.ArcTo(r, r, 0, SKPathArcSize.Small, SKPathDirection.CounterClockwise, webRight + r, bottomFlangeTop);

                    // F. Bottom Flange (Right edge)
                    path.LineTo(padding + b, bottomFlangeTop);
                    path.LineTo(padding + b, padding + h);                     // Bottom-Right Corner

                    // G. Bottom Flange (Bottom edge)
                    path.LineTo(padding, padding + h);                         // Bottom-Left Corner

                    // H. Bottom Flange (Left edge)
                    path.LineTo(padding, bottomFlangeTop);                     // Top-Left of bottom flange

                    // I. Bottom-Left Fillet
                    path.LineTo(webLeft - r, bottomFlangeTop);
                    path.ArcTo(r, r, 0, SKPathArcSize.Small, SKPathDirection.CounterClockwise, webLeft, bottomFlangeTop - r);

                    // J. Web (Left edge)
                    path.LineTo(webLeft, topFlangeBottom + r);

                    // K. Top-Left Fillet
                    path.ArcTo(r, r, 0, SKPathArcSize.Small, SKPathDirection.CounterClockwise, webLeft - r, topFlangeBottom);

                    // L. Top Flange (Left edge finish)
                    path.LineTo(padding, topFlangeBottom);
                    path.Close(); // Connects back to (padding, padding)

                    // 7. Draw the Path
                    canvas.DrawPath(path, paint);
                }
            }

            return bitmap;
        }
    }
}

