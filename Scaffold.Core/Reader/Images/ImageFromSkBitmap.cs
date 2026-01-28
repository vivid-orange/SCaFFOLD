using SkiaSharp;

namespace Scaffold.Reader.Images;

public class ImageFromSkBitmap : ICalcImage
{
    public ImageFromSkBitmap(SKBitmap bitmap) => Bitmap = bitmap;
    public SKBitmap Bitmap { get; }
    public SKBitmap GetImage() => Bitmap;
}
