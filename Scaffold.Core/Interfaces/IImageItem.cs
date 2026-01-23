using Scaffold.Core.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core

{
    public interface IImageItem : IContentItem
    {
        ICalcImage Image { get; }
    }
}
