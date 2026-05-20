
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetAllBookings
{
    public class GetAllBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetAllBookingsQuery, IReadOnlyList<DestinationBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IReadOnlyList<DestinationBookingDto>> Handle(
            GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(userId: null, status: request.Status);
            var bookings = await _unitOfWork.Repository<DestinationBooking>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<DestinationBookingDto>>(bookings);
        
    }
}
}
