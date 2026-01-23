namespace Scaffold.Core
{
    public abstract class Expression : IContentItem
    {
        public bool IncludeInSummary { get; } = true;

        public bool IsInLine { get; protected set; } = false;
    }
}
