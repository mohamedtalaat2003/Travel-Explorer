using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Common.Parameters
{
    public class FlightBookingSpecParams : PaginationParams
    {
        public BookingStatus? Status { get; set; }
    }
}
