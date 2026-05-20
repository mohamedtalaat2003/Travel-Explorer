
using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Commands.CreateFlightSchedule
{
    public class CreateFlightScheduleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateFlightScheduleCommand, FlightScheduleDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<FlightScheduleDto> Handle(CreateFlightScheduleCommand request, CancellationToken cancellationToken)
        {
            if (request.ArrivalTime <= request.DepartureTime)
            {
                throw new BadRequestException("Arrival time must be after departure time.");
            }

            var flightSchedule = _mapper.Map<FlightSchedule>(request);
            flightSchedule.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<FlightSchedule>().AddAsync(flightSchedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FlightScheduleDto>(flightSchedule);
        }
    }
}
