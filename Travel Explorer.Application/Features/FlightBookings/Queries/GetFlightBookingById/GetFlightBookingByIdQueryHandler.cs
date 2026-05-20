using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Flights.Bookings;

namespace Travel_Explorer.Application.Features.FlightBookings.Queries.GetFlightBookingById
{
    public class GetFlightBookingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetFlightBookingByIdQuery, FlightBookingDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<FlightBookingDto> Handle(GetFlightBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new FlightBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<FlightBooking>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(FlightBooking), request.Id);
            if (!_currentUserService.IsAdmin && booking.UserId != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }

            return _mapper.Map<FlightBookingDto>(booking);
        }
    }
}
