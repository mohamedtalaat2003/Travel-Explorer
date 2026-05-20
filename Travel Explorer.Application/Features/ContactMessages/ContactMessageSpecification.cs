using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.ContactMessages
{
    public class ContactMessageSpecification : BaseSpecification<ContactMessage>
    {
        public ContactMessageSpecification(int id) : base(C => C.Id == id)
        {
            AddInclude(C => C.User!);
        }

        public ContactMessageSpecification(ContactMessageSpecParams p, int? userId = null) : base()
        {
            if (p.IsRead.HasValue)
                AddCriteria(C => C.IsRead == p.IsRead.Value);

            if (userId.HasValue)
                AddCriteria(C => C.UserId == userId.Value);

            if (!string.IsNullOrWhiteSpace(p.Keyword))
            {
                var pattern = $"%{p.Keyword}%";
                AddCriteria(C => EF.Functions.ILike(C.FullName, pattern)
                              || EF.Functions.ILike(C.Email, pattern)
                              || EF.Functions.ILike(C.Subject, pattern)
                              || EF.Functions.ILike(C.Message, pattern));
            }

            AddOrderByDescending(C => C.CreatedAt);
            ApplyPaging((p.PageNumber - 1) * p.PageSize, p.PageSize);
        }
    }
}
