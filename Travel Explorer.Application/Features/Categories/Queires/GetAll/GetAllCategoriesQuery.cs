using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Categories.Queires.GetAll
{
    public record GetAllCategoriesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<CategoryDto>>;
    
}
