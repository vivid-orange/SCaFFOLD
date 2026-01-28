using System.IO;
using System.Text;
using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace Scaffold.Reader.Images;

public class ImageFromSkSvg : ICalcImage
{
    public ImageFromSkSvg(SKSvg svg) => Svg = svg;
    public ImageFromSkSvg(string svgText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(svgText);
        MemoryStream stream = new MemoryStream(bytes);
        Svg.Load(stream);
    }
    public SKBitmap Bitmap => GetImage();
    public SKSvg Svg { get; } = new SKSvg();
    public SKBitmap GetImage()
    {
        var image = SKImage.FromPicture(Svg.Picture,
            new SKSizeI((int)Svg.CanvasSize.Width, (int)Svg.CanvasSize.Height));
        return SKBitmap.FromImage(image);
    }
}
