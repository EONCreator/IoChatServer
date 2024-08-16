using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace IoChatServer.Services.Images;

public interface IIMageService
{
    Task<string> ConvertFromBase64(string base64EncodedImage);
}

public class ImageService : IIMageService
{
    public ImageService()
    {
        
    }

    public async Task<string> ConvertFromBase64(string base64EncodedImage)
    {
        var converted = base64EncodedImage.Replace("data:image/png;base64,", String.Empty);
        var imageBytes = Convert.FromBase64String(converted);
        
        Guid name = Guid.NewGuid();
        var path = Path.Combine(Directory.GetCurrentDirectory(), @"Assets/Images");
        using (var image = Image.Load(imageBytes))
        {
            image.Save(Path.Combine(path, $"{name}.png"));
            
            image.Mutate(x => x.Resize(256, 256));
            image.Save(Path.Combine(path, $"{name}_large.png"));
            
            image.Mutate(x => x.Resize(38, 38));
            image.Save(Path.Combine(path, $"{name}_medium.png"));
            
            image.Mutate(x => x.Resize(27, 27));
            image.Save(Path.Combine(path, $"{name}_small.png"));
        }

        return await Task.FromResult(name.ToString());
    }
}