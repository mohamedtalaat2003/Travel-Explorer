using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Categories.Commands.Delete
{
    public record DeleteCategoryCommand(int Id) : IRequest<bool>;
}
