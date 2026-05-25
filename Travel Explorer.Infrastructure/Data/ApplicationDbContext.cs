using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Travel_Explorer.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
    {
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<DestinationBooking> DestinationBookings { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<FlightSchedule> FlightSchedules { get; set; }
        public DbSet<FlightBooking> FlightBookings { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enable pg_trgm extension for fuzzy/trigram search
            builder.HasPostgresExtension("pg_trgm");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());               
        }
    }
}
