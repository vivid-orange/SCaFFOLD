using Scaffold.Core.Images;
using Scaffold.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core
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
