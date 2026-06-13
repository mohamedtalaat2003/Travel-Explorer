
namespace Travel_Explorer.Application.Features.Activities.Commands.DeleteActivity
{
    public class DeleteActivityCommandHandler(IUnitOfWork unitOfWork)
                : IRequestHandler<DeleteActivityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(
            DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            if(request.Id <= 0)
                throw new ArgumentException("Invalid activity ID.", nameof(request.Id));

            var activity = await _unitOfWork.Repository<Activity>().GetAsync(request.Id);
            if (activity == null)
                 throw new NotFoundException(nameof(Activity), request.Id);

            activity.IsDeleted = true;
            activity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        
    }
}
}
