using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.MarkAsRead
{
    public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, ContactMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MarkAsReadCommandHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ContactMessageDto?> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.Repository<ContactMessage>().GetAsync(request.Id);

            if (message == null) return null;

            message.IsRead = true;
            _unitOfWork.Repository<ContactMessage>().Update(message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ContactMessageDto>(message) ;
        }
    }
}
