
namespace Travel_Explorer.Application.Features.Activities.Queries.GetActivityById
{

    
    
    
    public record GetActivityByIdQuery(int Id) : IRequest<ActivityDto?>;

}
