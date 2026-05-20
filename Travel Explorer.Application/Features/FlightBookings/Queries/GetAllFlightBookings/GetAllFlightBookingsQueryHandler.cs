using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Flights.Bookings;

namespace Travel_Explorer.Application.Features.FlightBookings.Queries.GetAllFlightBookings
{
    public class GetAllFlightBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllFlightBookingsQuery, PaginatedResult<FlightBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<FlightBookingDto>> Handle(
            GetAllFlightBookingsQuery request, CancellationToken cancellationToken)
        {
            var p = request.Params;
            var spec = new FlightBookingSpecification(p);

            var totalCount = await _unitOfWork.Repository<FlightBooking>().CountAsync(spec);
            var bookings = await _unitOfWork.Repository<FlightBooking>().ListSpecAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<FlightBookingDto>>(bookings);
            return new PaginatedResult<FlightBookingDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }
    }
}
