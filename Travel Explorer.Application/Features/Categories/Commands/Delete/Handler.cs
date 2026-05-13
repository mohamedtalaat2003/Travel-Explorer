using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Categories.Commands.Delete
{
    public class Handler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            var category =await _unitOfWork.Repository<Category>().GetAsync(request.Id);

            if(category == null)
                throw new ArgumentException(nameof(category));

            try
            {
                category.IsDeleted = true;
                await _unitOfWork.Repository<Category>().Delete(category.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Exception occure when try to delete");
            }

        }
    }
}
