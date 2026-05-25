namespace Travel_Explorer.Application.Features.ContactMessages.DeleteContactMessage
{
    public record DeleteContactMessageCommand(int Id) : IRequest<bool>;

}
