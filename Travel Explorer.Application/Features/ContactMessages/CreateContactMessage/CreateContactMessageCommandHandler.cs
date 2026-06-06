using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage
{
    public class CreateContactMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateContactMessageCommand, ContactMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ContactMessageDto> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<ContactMessage>(request);
            message.UserId = _currentUserService.UserId;

            await _unitOfWork.Repository<ContactMessage>().AddAsync(message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            
            return _mapper.Map<ContactMessageDto>(message);
        }
    }
}
