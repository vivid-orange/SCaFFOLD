namespace Scaffold.Report
{
    public class ImageItem : ContentItem, IImageItem
    {
        public ICalcImage Image { get; }

        public ImageItem(ICalcImage image)
        {
            Image = image;
        }
        public ImageItem(ICalcImage image, bool isInLine)
        {
            Image = image;
            IsInLine = isInLine;
        }
    }
}
