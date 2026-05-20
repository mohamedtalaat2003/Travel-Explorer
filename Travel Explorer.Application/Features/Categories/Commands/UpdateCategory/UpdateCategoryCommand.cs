namespace Travel_Explorer.Application.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(
        int Id,
        string Name,
        string? Description,
        string? IconUrl
    ) : IRequest<CategoryDto>;
}
