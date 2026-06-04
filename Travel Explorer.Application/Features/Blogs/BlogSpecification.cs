using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Blogs
{
    
    
    
    
    public class BlogSpecification : BaseSpecification<Blog>
    {
        
        public BlogSpecification(int id)
            : base(b => b.Id == id  && b.IsPublished)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Category);
        }

        
        
        
        
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
