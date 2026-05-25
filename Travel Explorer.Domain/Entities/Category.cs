namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a category used to classify destinations and blog posts
    /// (e.g., "Beach", "Mountain", "Historical", "Adventure").
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// The display name of the category (e.g., "Beach", "Adventure").
        /// Must be unique across all categories.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An optional description explaining what this category covers.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// URL or path to an icon/image representing this category (optional).
        /// </summary>
        public string? IconUrl { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// All destinations that belong to this category.
        /// </summary>
        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();

        /// <summary>
        /// All blog posts that belong to this category.
        /// </summary>
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
