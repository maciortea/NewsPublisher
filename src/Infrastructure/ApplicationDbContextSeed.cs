using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure
{
    public sealed class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string publisherUserId = await EnsureUserAsync(userManager, "publisher@test.com", "P@ssword1");
            string userId = await EnsureUserAsync(userManager, "test@test.com", "P@ssword1");
            await EnsureRoleAsync(userManager, roleManager, "Publisher", publisherUserId);
            await EnsureRoleAsync(userManager, roleManager, "User", userId);
        }

        private static async Task<string> EnsureUserAsync(UserManager<IdentityUser> userManager, string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser { UserName = userName, Email = userName };
                await userManager.CreateAsync(user, password);
            }
            return user.Id;
        }

        private static async Task EnsureRoleAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string roleName, string userId)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}
