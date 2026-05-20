using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetMyBookings
{
    public class GetMyBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<GetMyBookingsQuery, IReadOnlyList<DestinationBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<IReadOnlyList<DestinationBookingDto>> Handle(
            GetMyBookingsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? 0;
            var spec = new DestinationBookingSpecification(userId, request.Status);
            var bookings = await _unitOfWork.Repository<DestinationBooking>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<DestinationBookingDto>>(bookings);
        
    }
}
}
