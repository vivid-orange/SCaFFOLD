using System.IO;
using SkiaSharp;

namespace Scaffold.Reader.Images.Drawing;

public static class DrawingUtiliy
{
    public static void Save(ICalcImage image, string filenameAndPath,
        SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        SKBitmap bitmap = image.GetImage();
        var canvas = new SKCanvas(bitmap);
        FileStream output = File.OpenWrite($"{filenameAndPath}.{format.ToString().ToLower()}");
        bitmap.Encode(output, format, quality);
        output.Close();
        canvas.Discard();
    }
}
