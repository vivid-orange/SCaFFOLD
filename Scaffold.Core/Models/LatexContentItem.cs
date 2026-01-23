namespace Scaffold.Core
{
    public class LatexItem : Expression, ILatexOutputItem
    {
        public string Latex { get; }

        public LatexItem(string latex)
        {
            Latex = latex;
        }

        public LatexItem(string latex, bool isInLine)
        {
            Latex = latex;
            IsInLine = isInLine;
        }
    }
}
