namespace Travel_Explorer.Application.Common.Parameters
{
    public class FlightScheduleSpecParams : PaginationParams
    {
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public DateTime? DepartureDate { get; set; }
    }
}
