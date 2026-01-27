using Scaffold.Core;
using SkiaSharp;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SCaFFOLD_Desktop
{
    public class ExpressionViewModel : ViewModelBase
    {
        private readonly IContentItem _model;
        private ImageSource _cachedImageSource;

        public ExpressionViewModel(IContentItem model)
        {
            _model = model;
        }

        // --- Layout Flags ---
        // We cast to the abstract base 'Expression' to access IsInLine
        public bool IsInLine => (_model as ContentItem)?.IsInLine ?? false;

        // --- Type Flags ---
        public bool IsText => _model is ITextItem;
        public bool IsLatex => _model is ILatexItem;
        public bool IsImage => _model is IImageItem;

        // --- Content Properties ---
        public string Content
        {
            get
            {
                if (_model is ITextItem textItem) return textItem.Text;
                if (_model is ILatexItem latexItem) return latexItem.Latex;
                return string.Empty;
            }
        }

        public ImageSource Image
        {
            get
            {
                if (_cachedImageSource != null) return _cachedImageSource;

                if (_model is IImageItem imageItem && imageItem.Image != null)
                {
                    try
                    {
                        SKBitmap skBitmap = imageItem.Image.GetImage();
                        if (skBitmap != null)
                        {
                            using (var image = SKImage.FromBitmap(skBitmap))
                            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                            {
                                var bitmapImage = new BitmapImage();
                                using (var stream = new MemoryStream(data.ToArray()))
                                {
                                    bitmapImage.BeginInit();
                                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmapImage.StreamSource = stream;
                                    bitmapImage.EndInit();
                                    bitmapImage.Freeze();
                                }
                                _cachedImageSource = bitmapImage;
                            }
                        }
                    }
                    catch
                    {
                        _cachedImageSource = null;
                    }
                }
                return _cachedImageSource;
            }
        }
    }
}
