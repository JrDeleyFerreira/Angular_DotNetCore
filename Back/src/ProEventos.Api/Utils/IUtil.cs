namespace ProEventos.Api.Utils;

public interface IUtil
{
    Task<string> SaveImage(IFormFile image, string folder);
    void DeleteImage(string folder, string imageName);
}
