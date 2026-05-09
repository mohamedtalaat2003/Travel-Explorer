using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Travel_Explorer.Infrastructure.Persistence.Seed
{
    public class RoleSeeder
    {
        public static async Task  SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles =   {"Admin" , "Traveler" ,"Author" };

            foreach (var role in roles)
            {
                if(! await roleManager.RoleExistsAsync(role))
                {
                  await  roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

    }
}
