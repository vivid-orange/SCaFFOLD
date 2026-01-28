using SkiaSharp;

namespace Scaffold.Reader.Images.Drawing;

public static class Colours
{
    public static SKColor ConcreteFill { get; set; } = GreyMedium;
    public static SKColor ConcreteOutline { get; set; } = GreyDark;
    public static SKColor RebarFill { get; set; } = GreyDark;
    public static SKColor SteelFill { get; set; } = new SKColor(216, 190, 189);
    public static SKColor SteelOutline { get; set; } = Red;

    public static SKColor Green => new SKColor(72, 166, 95);
    public static SKColor Red => new SKColor(237, 61, 59);
    public static SKColor Blue => new SKColor(39, 95, 171);

    public static SKColor BlueDark => new SKColor(32, 77, 140);
    public static SKColor BlueLight => new SKColor(129, 161, 206);
    public static SKColor BlueMedium => new SKColor(74, 120, 185);

    public static SKColor GreyDark => new SKColor(59, 61, 63);
    public static SKColor GreyLight => new SKColor(247, 249, 251);
    public static SKColor GreyMedium => new SKColor(221, 221, 220);

    public static SKColor OrangeDark => new SKColor(237, 63, 0);
    public static SKColor OrangeLight => new SKColor(242, 128, 86);
    public static SKColor OrangeMedium => new SKColor(223, 84, 41);

    public static SKColor TealDark => new SKColor(0, 102, 102);
    public static SKColor TealLight => new SKColor(46, 187, 184);
    public static SKColor TealMedium => new SKColor(0, 153, 153);

    public static SKColor PurpleDark => new SKColor(94, 83, 113);
    public static SKColor PurpleLight => new SKColor(182, 161, 226);
    public static SKColor PurpleMedium => new SKColor(151, 120, 213);
}
