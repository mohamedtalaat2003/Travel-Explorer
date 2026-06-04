using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Infrastructure.Persistence.Seed
{
    
    
    
    
    
    public static class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            var userName = configuration["AdminUser:UserName"] ?? "admin";
            var email = configuration["AdminUser:Email"] ?? "admin@travelexplorer.com";
            var password = configuration["AdminUser:Password"] ?? "Admin@123";
            var fullName = configuration["AdminUser:FullName"] ?? "System Administrator";

            
            if (await userManager.FindByNameAsync(userName) is not null)
                return;

            var admin = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FullName = fullName,
                Role = "Admin",
                Status = AccountStatus.Approved,
                requestToBeAuthor = RequestToBeAuthor.Rejected,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
