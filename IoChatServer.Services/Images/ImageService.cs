using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace IoChatServer.Services.Images;

struct ImageSize
{
    public int Width { get; }
    public int Height { get; }

    public ImageSize(int width, int height)
    {
        Width = width;
        Height = height;
    }
}

struct ImageType
{
    public string Name { get; }
    public ImageSize Size { get; }

    public ImageType(string name, ImageSize size)
    {
        Name = name;
        Size = size;
    }
}

public interface IIMageService
{
    Task<string> ConvertFromBase64(string base64EncodedImage);
}

public class ImageService : IIMageService
{
    public async Task<string> ConvertFromBase64(string base64EncodedImage)
    {
        var converted = base64EncodedImage.Replace("data:image/png;base64,", String.Empty);
        var imageBytes = Convert.FromBase64String(converted);
        
        Guid name = Guid.NewGuid();
        var path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets/Images");
        
        using (var image = Image.Load(imageBytes))
        {
            image.Save(Path.Combine(path, $"{name}.png"));

            var imageTypes = new List<ImageType>()
            {
                new("large", new ImageSize(256, 256)),
                new("medium", new ImageSize(38, 38)),
                new("small", new ImageSize(27, 27))
            };

            foreach (var imageType in imageTypes)
            {
                image.Mutate(x => 
                    x.Resize(imageType.Size.Width, imageType.Size.Height));
                image.Save(Path.Combine(path, $"{name}_{imageType.Name}.png"));
            }
        }

        return await Task.FromResult(name.ToString());
    }
}