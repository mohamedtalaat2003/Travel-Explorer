
namespace Travel_Explorer.Application.Features.Activities
{
    
    
    
    public class ActivitySpecification : BaseSpecification<Activity>
    {
        
        
        
        public ActivitySpecification(int id)
            : base(a => a.Id == id)
        {
            AddInclude(a => a.Destination);
        }

        
        
        
        public ActivitySpecification(int? destinationId = null)
            : base(a => !destinationId.HasValue || a.DestinationId == destinationId.Value)
        {
            AddInclude(a => a.Destination);
            AddOrderBy(a => a.Id);

        }

        
        
        
        public ActivitySpecification(int? destinationId, int pageNumber, int pageSize)
            : base(a => !destinationId.HasValue || a.DestinationId == destinationId.Value)
        {
            AddInclude(a => a.Destination);
            AddOrderBy(a => a.Id);
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
