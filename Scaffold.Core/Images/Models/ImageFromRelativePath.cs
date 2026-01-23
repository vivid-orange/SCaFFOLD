using SkiaSharp;

namespace Scaffold.Reader;

public class ImageFromRelativePath : ICalcImage
{
    public string RelativePathName { get; }
    public IAssemblyImageReader ImageReader { get; set; }

    public ImageFromRelativePath(string relativePathName)
    {

        RelativePathName = relativePathName;

    }


    public SKBitmap GetImage()
    {
        AssemblyImage assemblyImage = ImageReader.Images.FirstOrDefault(x => x.Path.Contains(RelativePathName));
        if (assemblyImage == null)
            throw new Exception($"File from relative path '{RelativePathName}' not found.");

        return SKBitmap.Decode(assemblyImage.Data);
    }
}
