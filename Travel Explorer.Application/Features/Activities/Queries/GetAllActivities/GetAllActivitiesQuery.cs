
using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Activities.Queries.GetAllActivities
{

    
    
    
    public record GetAllActivitiesQuery(int ? destinationId =null , int PageNumber=1 , int PageSize=10 ) : IRequest<PaginatedResult<ActivityDto>>;

}
