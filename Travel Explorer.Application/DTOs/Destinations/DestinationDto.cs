namespace Travel_Explorer.Application.DTOs.Destinations
{
    public class DestinationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal PricePerNight { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string? ThumbnailUrl { get; set; }
        public List<string> ImageUrls { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
