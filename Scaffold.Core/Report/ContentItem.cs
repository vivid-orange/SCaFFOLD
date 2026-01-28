namespace Scaffold.Report
{
    public abstract class ContentItem : IContentItem
    {
        public bool IncludeInSummary { get; } = true;

        public bool IsInLine { get; protected set; } = false;
    }
}
