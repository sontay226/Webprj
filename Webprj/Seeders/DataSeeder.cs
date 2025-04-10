using Microsoft.AspNetCore.Identity;
using Webprj.Models;

namespace Webprj.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedRoles( RoleManager<IdentityRole<int>> roleManager )
        {
            string[] roles = { "Admin" , "User" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
                }
            }
        }
        public static async Task SeedAdminUser( UserManager<Customer> userManager , RoleManager<IdentityRole<int>> roleManager )
        {
            string adminEmail = "sontaypham2206@gmail.com";
            string adminPassword = "226005";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new Customer
                {
                    UserName = adminEmail ,
                    Email = adminEmail ,
                    CustomerName = "Phạm Sơn Tây" ,
                    EmailConfirmed = true ,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser , adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Không thể tạo user admin: " + string.Join(", " , result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser , "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser , "Admin");
            }
        }

    }
}
