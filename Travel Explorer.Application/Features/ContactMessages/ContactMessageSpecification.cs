namespace Travel_Explorer.Application.Features.ContactMessages
{
    public class ContactMessageSpecification : BaseSpecification<ContactMessage>
    {
        public ContactMessageSpecification(int id) : base(C => C.Id == id)
        {
            AddInclude(C => C.User!);
        }

        public ContactMessageSpecification(int? pageNumber = null, int? pageSize = null, bool? isRead = null, int? userId = null) : base()
        {
            if (isRead.HasValue)
                AddCriteria(C => C.IsRead == isRead.Value);

            if (userId.HasValue)
                AddCriteria(C => C.UserId == userId.Value);

            AddOrderByDescending(C => C.CreatedAt);
            
            if (pageNumber.HasValue && pageSize.HasValue)
                ApplyPaging((pageNumber.Value - 1) * pageSize.Value, pageSize.Value);
        }
    }
}
