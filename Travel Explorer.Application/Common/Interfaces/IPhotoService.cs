namespace Travel_Explorer.Application.Common.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoUploadResult> UploadPhotoAsync(Stream fileStream, string fileName);
        Task<bool> DeletePhotoAsync(string publicId);
    }
}
