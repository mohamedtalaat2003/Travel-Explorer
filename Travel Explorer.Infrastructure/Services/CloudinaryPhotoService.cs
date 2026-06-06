using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Travel_Explorer.Application.Common.Interfaces;

namespace Travel_Explorer.Infrastructure.Services
{
    public class CloudinaryPhotoService : IPhotoService
    {
        private readonly Cloudinary? _cloudinary;
        private readonly bool _isConfigured;

        public CloudinaryPhotoService(IOptions<CloudinarySettings> config)
        {
            var settings = config?.Value;
            if (settings != null && 
                !string.IsNullOrWhiteSpace(settings.CloudName) &&
                !string.IsNullOrWhiteSpace(settings.ApiKey) &&
                !string.IsNullOrWhiteSpace(settings.ApiSecret))
            {
                var acc = new Account(
                    settings.CloudName,
                    settings.ApiKey,
                    settings.ApiSecret
                );

                _cloudinary = new Cloudinary(acc);
                _isConfigured = true;
            }
            else
            {
                _isConfigured = false;
            }
        }

        public async Task<PhotoUploadResult> UploadPhotoAsync(Stream fileStream, string fileName)
        {
            if (!_isConfigured || _cloudinary == null)
            {
                return new PhotoUploadResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Cloudinary is not configured. Please set 'Cloudinary:CloudName', 'Cloudinary:ApiKey', and 'Cloudinary:ApiSecret' in Azure settings or appsettings.json."
                };
            }

            var uploadResult = new ImageUploadResult();

            if (fileStream.Length > 0)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, fileStream),
                    Transformation = new Transformation().Height(800).Width(1200).Crop("limit")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
            {
                return new PhotoUploadResult
                {
                    IsSuccess = false,
                    ErrorMessage = uploadResult.Error.Message
                };
            }

            return new PhotoUploadResult
            {
                IsSuccess = true,
                Url = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };
        }

        public async Task<bool> DeletePhotoAsync(string publicId)
        {
            if (!_isConfigured || _cloudinary == null)
            {
                return false;
            }

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }
    }
}
