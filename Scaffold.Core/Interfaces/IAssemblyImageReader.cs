namespace Scaffold.Reader;

public interface IAssemblyImageReader
{
    IReadOnlyList<AssemblyImage> Images { get; }
}
