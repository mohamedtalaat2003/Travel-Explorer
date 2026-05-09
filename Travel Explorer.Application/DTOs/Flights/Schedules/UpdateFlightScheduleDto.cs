
namespace Travel_Explorer.Application.DTOs.Flights.Schedules
{
    public class UpdateFlightScheduleDto : CreateFlightScheduleDto
    {
        [Required]
        public int Id { get; set; }
    }
}
