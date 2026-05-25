
using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Commands.UpdateFlightSchedule
{
    public class UpdateFlightScheduleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateFlightScheduleCommand, FlightScheduleDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<FlightScheduleDto> Handle(UpdateFlightScheduleCommand request, CancellationToken cancellationToken)
        {
            var spec = new FlightScheduleSpecification(request.Id);
            var flightSchedule = await _unitOfWork.Repository<FlightSchedule>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(FlightSchedule), request.Id);
            if (request.ArrivalTime <= request.DepartureTime)
            {
                throw new BadRequestException("Arrival time must be after departure time.");
            }

            _mapper.Map(request, flightSchedule);
            flightSchedule.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<FlightSchedule>().Update(flightSchedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FlightScheduleDto>(flightSchedule);
        }
    }
}
