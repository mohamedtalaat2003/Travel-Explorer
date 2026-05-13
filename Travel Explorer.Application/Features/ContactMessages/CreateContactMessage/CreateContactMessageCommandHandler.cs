using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage
{
    public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand, ContactMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateContactMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ContactMessageDto> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<ContactMessage>(request);

            await _unitOfWork.Repository<ContactMessage>().AddAsync(message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return the mapped DTO directly from the saved entity
            return _mapper.Map<ContactMessageDto>(message);
        }
    }
}
