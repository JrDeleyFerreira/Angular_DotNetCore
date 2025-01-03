namespace ProEventos.Api.Utils;

public class Util : IUtil
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public Util(IWebHostEnvironment hostEnvironment)
        => _hostEnvironment = hostEnvironment;

    public async Task<string> SaveImage(IFormFile imageFile, string folder)
    {
        var imageName = new string(
            Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray())
            .Replace(' ', '-');

        imageName = $"{imageName}{DateTime.UtcNow:yymmssfff}{Path.GetExtension(imageFile.FileName)}";

        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{folder}", imageName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return imageName;
    }

    public void DeleteImage(string folder, string imageName)
    {
        if (!string.IsNullOrEmpty(imageName))
        {
            var imagePath =
                Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{folder}", imageName);

            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }
    }
}
