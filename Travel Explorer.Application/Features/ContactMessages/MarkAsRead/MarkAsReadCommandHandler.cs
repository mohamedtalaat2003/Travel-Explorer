using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.MarkAsRead
{
    public class MarkAsReadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<MarkAsReadCommand, ContactMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ContactMessageDto> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.Repository<ContactMessage>().GetAsync(request.Id) ?? throw new NotFoundException(nameof(ContactMessage), request.Id);
            message.IsRead = true;
            _unitOfWork.Repository<ContactMessage>().Update(message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ContactMessageDto>(message) ;
        }
    }
}
