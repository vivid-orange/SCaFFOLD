using Scaffold.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core
{
    public class TextItem : Expression, ITextOutputItem
    {
        public string Text { get; }

        public TextItem(string text)
        {
            Text = text;
        }

        public TextItem(string text, bool isInLine)
        {
            Text = text;
            IsInLine = isInLine;
        }
    }
}
