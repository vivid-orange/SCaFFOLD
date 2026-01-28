using System.IO;

namespace Scaffold.Reader.Images;

public class AssemblyImageReader : IAssemblyImageReader
{
    private readonly List<AssemblyImage> _images = new();

    public AssemblyImageReader(string relativePathName, string folderPath)
    {
        string fullFilePath = Path.Combine(folderPath, relativePathName);
        using FileStream stream = File.OpenRead(fullFilePath);

        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        _images.Add(new AssemblyImage { Path = relativePathName, Data = memoryStream.ToArray() });
    }

    public IReadOnlyList<AssemblyImage> Images => _images;
}
