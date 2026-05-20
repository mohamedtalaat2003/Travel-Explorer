using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Categories.Queries.GetAllCategories
{
    /// <summary>
    /// Returns a paginated list of active categories.
    /// </summary>
    public record GetAllCategoriesQuery(CategorySpecParams Params) : IRequest<PaginatedResult<CategoryDto>>;
}
