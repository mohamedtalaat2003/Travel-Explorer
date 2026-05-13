using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Categories.Queires.Get
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>;
    
}
