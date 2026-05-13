using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Categories
{
    public class CategorySpecification :BaseSpecification<Category>
    {
        public CategorySpecification(int Id) :base(b=> b.Id == Id && !b.IsDeleted)
        {
            AddInclude(c => c.Destinations);
            AddInclude(c => c.Blogs);
        }
    }
}
