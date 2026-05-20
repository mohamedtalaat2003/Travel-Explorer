namespace Travel_Explorer.Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>;
}
