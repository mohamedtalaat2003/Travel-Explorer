namespace Travel_Explorer.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int Id) : IRequest<bool>;
}
