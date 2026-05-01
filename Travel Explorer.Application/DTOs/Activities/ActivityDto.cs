namespace Travel_Explorer.Application.DTOs.Activities
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<string> ImageUrls { get; set; }
        public int DestinationId { get; set; }
        public string DestinationName { get; set; }
    }
}
