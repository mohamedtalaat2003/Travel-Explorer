namespace Travel_Explorer.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<string> failures)
            : this()
        {
            Errors.Add("General", failures.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
