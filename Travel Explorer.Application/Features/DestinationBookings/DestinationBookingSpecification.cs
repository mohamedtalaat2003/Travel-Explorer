
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.DestinationBookings
{
    
    
    
    public class DestinationBookingSpecification : BaseSpecification<DestinationBooking>
    {
        
        
        
        public DestinationBookingSpecification(int id)
            : base(b => b.Id == id )
        {
            AddInclude(b => b.User);
            AddInclude(b => b.Destination);
        }

        
        
        
        public DestinationBookingSpecification(int paymentId, bool isPayment)
            : base(b => b.PaymentId == paymentId)
        {
            AddInclude(b => b.User);
            AddInclude(b => b.Destination);
        }

        
        
        
        public DestinationBookingSpecification(int? userId, string? status)
             : base(b => (!userId.HasValue || b.UserId == userId.Value))
        {
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<BookingStatus>(status, true, out var parsedStatus))
                {
                    AddCriteria(b => b.Status == parsedStatus);
                }
                else
                {
                    
                    AddCriteria(b => false);
                }
            }

            AddInclude(b => b.User);
            AddInclude(b => b.Destination);
            AddOrderByDescending(b => b.CreatedAt);
        }
    }
}
