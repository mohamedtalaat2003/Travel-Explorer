
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetMyBookings
{
    public class GetMyBookingsQueryHandler
        : IRequestHandler<GetMyBookingsQuery, IReadOnlyList<DestinationBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMyBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DestinationBookingDto>> Handle(
            GetMyBookingsQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.UserId, request.Status);
            var bookings = await _unitOfWork.Repository<DestinationBooking>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<DestinationBookingDto>>(bookings);
        
    }
}
}
