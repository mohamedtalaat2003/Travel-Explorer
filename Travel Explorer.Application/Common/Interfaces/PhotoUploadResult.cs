namespace Travel_Explorer.Application.Common.Interfaces
{
    public class PhotoUploadResult
    {
        public string? PublicId { get; set; }
        public string? Url { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
