
namespace Travel_Explorer.Application.Features.Activities.Queries.GetActivityById
{

    /// <summary>
    /// Returns a single activity by its ID.
    /// </summary>
    public record GetActivityByIdQuery(int Id) : IRequest<ActivityDto?>;

}
