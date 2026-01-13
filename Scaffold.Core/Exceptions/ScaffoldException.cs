namespace Scaffold.Core.Exceptions;

public class ScaffoldException : Exception
{
    /// <exception cref="ScaffoldException">
    ///     If anything else goes wrong, typically due to a bug or unhandled case.
    ///     We wrap exceptions in <see cref="ScaffoldException" /> to allow you to distinguish
    ///     Scaffold.Core exceptions from other exceptions.
    /// </exception>
    public ScaffoldException(string message) : base(message) { }
}
