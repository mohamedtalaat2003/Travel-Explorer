namespace Travel_Explorer.Application.DTOs.Users
{
    public record AdminStatisticsDto(
        int TotalUsers,
        int ActiveUsers,
        int TotalBookings
    );
}
