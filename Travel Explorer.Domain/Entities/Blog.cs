using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Domain.Common;

namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a blog post/article on the Travel Explorer platform.
    /// Written by users with the ContentWriter role to share travel tips and guides.
    /// </summary>
    public class Blog : BaseEntity
    {
        /// <summary>
        /// The title/headline of the blog post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The full body content/text of the blog post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// URL or path to the cover/featured image for this blog post.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Whether this blog post is published and visible to all users.
        /// If false, the post is in draft mode and only visible to the author.
        /// </summary>
        public bool IsPublished { get; set; } = false;

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the user who authored this blog post.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Foreign key to the category this blog post belongs to (optional).
        /// Null if the post is uncategorized.
        /// </summary>
        public int? CategoryId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The user who authored this blog post.
        /// </summary>
        public ApplicationUser Author { get; set; }

        /// <summary>
        /// The category this blog post belongs to (nullable).
        /// </summary>
        public Category? Category { get; set; }
    }
}

