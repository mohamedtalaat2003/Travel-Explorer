using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Categories
{
    
    
    
    public class CategorySpecification : BaseSpecification<Category>
    {
        
        public CategorySpecification(int id)
            : base(c => c.Id == id)
        {
            AddInclude(c => c.Destinations);
            AddInclude(c => c.Blogs);
            ApplySplitQuery();
        }

        
        public CategorySpecification(string name)
            : base(c => c.Name == name)
        {
        }

        
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
