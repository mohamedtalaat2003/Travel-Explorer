namespace Travel_Explorer.Application.Common.Exceptions
{
    public class UserBlockedException : ForbiddenAccessException
    {
        public UserBlockedException() : base("Your account has been blocked. You cannot perform this operation.")
        {
        }

        public UserBlockedException(string message) : base(message)
        {
        }
    }
}
