namespace Scaffold.Report
{
    public class TextItem : ContentItem, ITextItem
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
