namespace Travel_Explorer.Domain.Enums
{
    public enum BookingStatus
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3,
        Completed = 4,
        Refunded = 5
    }

    public enum PaymentStatus
    {
        Unpaid = 1,
        Partial = 2,
        Paid = 3,
        Failed = 4,
        Refunded = 5
    }

    public enum FlightClass
    {
        Economy = 1,
        Business = 2,
        FirstClass = 3
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public enum AccountStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public enum RequestToBeAuthor
    {
        Pending = 1,
        Approved,
        Rejected
    }
}
