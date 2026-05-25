using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<GetBookingByIdQuery, DestinationBookingDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<DestinationBookingDto?> Handle(
            GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(DestinationBooking), request.Id);

            // Non-admin users can only view their own bookings
            if (!_currentUserService.IsAdmin && booking.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("You are not authorized to view this booking.");

            return _mapper.Map<DestinationBookingDto>(booking);
        
    }
}
}
