using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Bookings
{
    public class DestinationBookingDto
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public string? Notes { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int DestinationId { get; set; }
        public string DestinationName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
