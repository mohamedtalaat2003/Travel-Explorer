
namespace Travel_Explorer.Application.Features.Activities.Commands.DeleteActivity
{

    /// <summary>
    /// Soft-deletes an activity by ID.
    /// </summary>
    public record DeleteActivityCommand(int Id) : IRequest<bool>;

}
