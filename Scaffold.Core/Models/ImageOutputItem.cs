namespace Scaffold.Core
{
    public class ImageOutputItem : Expression, IImageOutputItem
    {
        public ICalcImage Image { get; }

        public ImageOutputItem(ICalcImage image)
        {
            Image = image;
        }
        public ImageOutputItem(ICalcImage image, bool isInLine)
        {
            Image = image;
            IsInLine = isInLine;
        }
    }
}
