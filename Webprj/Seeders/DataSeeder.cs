using Microsoft.AspNetCore.Identity;

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
    }
}
