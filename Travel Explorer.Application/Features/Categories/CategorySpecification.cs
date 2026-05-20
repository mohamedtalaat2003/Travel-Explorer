using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Categories
{
    /// <summary>
    /// Unified specification for querying Categories.
    /// </summary>
    public class CategorySpecification : BaseSpecification<Category>
    {
        /// <summary>Single category by ID with Destinations and Blogs included.</summary>
        public CategorySpecification(int id)
            : base(c => c.Id == id)
        {
            AddInclude(c => c.Destinations);
            AddInclude(c => c.Blogs);
            ApplySplitQuery();
        }

        /// <summary>Check uniqueness by name (excludes soft-deleted).</summary>
        public CategorySpecification(string name)
            : base(c => c.Name == name)
        {
        }

        /// <summary>Paginated list of all active categories.</summary>
        public CategorySpecification(CategorySpecParams p)
            : base()
        {
            if (!string.IsNullOrWhiteSpace(p.Name))
            {
                AddCriteria(c => EF.Functions.ILike(c.Name, $"%{p.Name}%"));
            }

            AddInclude(c => c.Destinations);
            AddInclude(c => c.Blogs);
            AddOrderBy(c => c.Id);
            ApplyPaging((p.PageNumber - 1) * p.PageSize, p.PageSize);
            ApplySplitQuery();
        }
    }
}
