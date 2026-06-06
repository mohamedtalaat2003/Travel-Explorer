
namespace Travel_Explorer.Application.Features.Reviews
{
    
    
    
    public class ReviewSpecification : BaseSpecification<Review>
    {
        
        
        
        public ReviewSpecification(int id)
            : base(r => r.Id == id)
        {
            AddInclude(r => r.User);
        }

        
        
        
        
        public ReviewSpecification(int destinationId, bool filterByDestination)
            : base(r => r.DestinationId == destinationId)
        {
            AddInclude(r => r.User);
            AddOrderByDescending(r => r.CreatedAt);
        }
}
}
