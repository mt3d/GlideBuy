namespace GlideBuy.Services.Media
{
    public interface IThumbService
    {
        Task SaveThumbAsync(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary);

        Task<string> GetThumbLocalPathByFileNameAsync(string thumbFileName);

        Task<bool> GeneratedThumbExistsAsync(string thumbFilePath, string thumbFileName)
    }
}
