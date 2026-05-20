using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Blogs
{
    /// <summary>
    /// Unified specification for querying Blogs.
    /// Supports single lookup, filtering by author/category/keyword, and full pagination.
    /// </summary>
    public class BlogSpecification : BaseSpecification<Blog>
    {
        /// <summary>Single published blog by ID.</summary>
        public BlogSpecification(int id)
            : base(b => b.Id == id  && b.IsPublished)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Category);
        }

        /// <summary>
        /// Paginated, filtered list of published blogs.
        /// Uses PostgreSQL ILIKE for case-insensitive fuzzy search (accelerated by GIN trigram indexes).
        /// </summary>
        public BlogSpecification(BlogSpecParams p)
            : base(b =>  b.IsPublished)
        {
            if (!string.IsNullOrWhiteSpace(p.Keyword))
            {
                var pattern = $"%{p.Keyword}%";
                AddCriteria(b => EF.Functions.ILike(b.Title, pattern)
                              || EF.Functions.ILike(b.Content, pattern));
            }

            if (p.AuthorId.HasValue)
                AddCriteria(b => b.AuthorId == p.AuthorId.Value);

            if (p.CategoryId.HasValue)
                AddCriteria(b => b.CategoryId == p.CategoryId.Value);

            AddInclude(b => b.Author);
            AddInclude(b => b.Category);
            AddOrderBy(b => b.Id);
            ApplyPaging((p.PageNumber - 1) * p.PageSize, p.PageSize);
        }
    }
}
