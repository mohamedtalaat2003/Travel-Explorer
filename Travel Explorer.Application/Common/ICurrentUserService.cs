namespace Travel_Explorer.Application.Common
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        bool IsAdmin { get; }
    }
}
