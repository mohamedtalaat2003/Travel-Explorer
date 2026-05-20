namespace Travel_Explorer.Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(
        string Name,
        string? Description,
        string? IconUrl
    ) : IRequest<CategoryDto>;
}
