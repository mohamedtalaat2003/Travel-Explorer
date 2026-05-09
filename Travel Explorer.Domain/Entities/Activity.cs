
namespace Travel_Explorer.Domain.Entities
{
    public class Activity : BaseEntity
    {
        /// <summary>
        /// Name of activity (e.g , Diving)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An optional description explaining what this activity covers.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// suitable icon to show in frontend 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// All images to show how acivity do 
        /// </summary>
        public List<string> ImageUrls { get; set; }

        /// <summary>
        /// Foreign key of Destination that provide this activity 
        /// </summary>
        public int DestinationId { get; set; }

        /// <summary>
        /// Navigation property to show any property or details about of Destination
        /// </summary>
        public Destination Destination { get; set; }
    }
}
