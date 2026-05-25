using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Flights.Bookings;

namespace Travel_Explorer.Application.Features.FlightBookings.Queries.GetMyFlightBookings
{
    public class GetMyFlightBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetMyFlightBookingsQuery, PaginatedResult<FlightBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<PaginatedResult<FlightBookingDto>> Handle(
            GetMyFlightBookingsQuery request, CancellationToken cancellationToken)
        {
            var p = request.Params;
            var userId = _currentUserService.UserId ?? 0;
            var spec = new FlightBookingSpecification(p, userId);

            var totalCount = await _unitOfWork.Repository<FlightBooking>().CountAsync(spec);
            var bookings = await _unitOfWork.Repository<FlightBooking>().ListSpecAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<FlightBookingDto>>(bookings);
            return new PaginatedResult<FlightBookingDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }
    }
}
