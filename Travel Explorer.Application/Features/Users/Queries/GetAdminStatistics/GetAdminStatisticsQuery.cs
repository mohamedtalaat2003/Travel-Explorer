using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Domain.Common;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Enums;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Application.Features.Users.Queries.GetAdminStatistics
{
    public record GetAdminStatisticsQuery : IRequest<AdminStatisticsDto>;

    public class GetAdminStatisticsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAdminStatisticsQuery, AdminStatisticsDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AdminStatisticsDto> Handle(GetAdminStatisticsQuery request, CancellationToken cancellationToken)
        {
            var totalUsersSpec = new BaseSpecification<ApplicationUser>();
            var totalUsers = await _unitOfWork.Repository<ApplicationUser>().CountAsync(totalUsersSpec);

            var activeUsersSpec = new BaseSpecification<ApplicationUser>(u =>
                !u.IsDeleted &&
                !u.IsBlocked &&
                u.Status == AccountStatus.Approved);
            var activeUsers = await _unitOfWork.Repository<ApplicationUser>().CountAsync(activeUsersSpec);

            var totalDestBookingsSpec = new BaseSpecification<DestinationBooking>(b => true);
            var totalDestBookings = await _unitOfWork.Repository<DestinationBooking>().CountAsync(totalDestBookingsSpec);

            var totalFlightBookingsSpec = new BaseSpecification<FlightBooking>(b => true);
            var totalFlightBookings = await _unitOfWork.Repository<FlightBooking>().CountAsync(totalFlightBookingsSpec);

            var totalBookings = totalDestBookings + totalFlightBookings;

            return new AdminStatisticsDto(totalUsers, activeUsers, totalBookings);
        }
    }
}
