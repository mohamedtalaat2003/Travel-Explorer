using Travel_Explorer.Application.Common.Interfaces;

namespace Travel_Explorer.Controllers
{
    
    
    
    [ApiController]
    [Route("api/Upload")]
    [Produces("application/json")]
    [Authorize]
    public class UploadController(IPhotoService photoService) : ControllerBase
    {
        private readonly IPhotoService _photoService = photoService;
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        private const long MaxFileSize = 5 * 1024 * 1024; 

        
        
        
        [HttpPost("image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (file.Length > MaxFileSize)
                return BadRequest("File size exceeds 5MB limit.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return BadRequest("Invalid image format. Supported formats: JPG, JPEG, PNG, WEBP.");

            using var stream = file.OpenReadStream();
            var uploadResult = await _photoService.UploadPhotoAsync(stream, file.FileName);

            if (!uploadResult.IsSuccess)
                return BadRequest(uploadResult.ErrorMessage);

            return Ok(new { url = uploadResult.Url, publicId = uploadResult.PublicId });
        }

        
        
        
        [HttpPost("images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadImages(IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");

            var uploadedUrls = new List<object>();

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                if (file.Length > MaxFileSize)
                    return BadRequest($"File '{file.FileName}' exceeds 5MB limit.");

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext))
                    return BadRequest($"File '{file.FileName}' has an invalid image format. Supported formats: JPG, JPEG, PNG, WEBP.");

                using var stream = file.OpenReadStream();
                var uploadResult = await _photoService.UploadPhotoAsync(stream, file.FileName);

                if (!uploadResult.IsSuccess)
                    return BadRequest($"Failed to upload '{file.FileName}': {uploadResult.ErrorMessage}");

                uploadedUrls.Add(new { url = uploadResult.Url, publicId = uploadResult.PublicId });
            }

            return Ok(uploadedUrls);
        }
    }
}
