﻿using ApplicationCore.Entities.ArticleAggregate;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure
{
    public sealed class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string publisherUserId = await EnsureUserAsync(userManager, "publisher@domain.com", "P@ssword1");
            string oneUserId = await EnsureUserAsync(userManager, "one@domain.com", "P@ssword1");
            string anotherUserId = await EnsureUserAsync(userManager, "another@domain.com", "P@ssword1");
            await EnsureRoleAsync(roleManager, "User");
            await EnsureRoleAsync(roleManager, "Publisher");
            await AssignRolesToUserAsync(userManager, oneUserId, "User");
            await AssignRolesToUserAsync(userManager, anotherUserId, "User");
            await AssignRolesToUserAsync(userManager, publisherUserId, "Publisher");
            await EnsureArticlesAsync(db, publisherUserId);
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

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task AssignRolesToUserAsync(UserManager<IdentityUser> userManager, string userId, params string[] roles)
        {
            var user = await userManager.FindByIdAsync(userId);
            foreach (string role in roles)
            {
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        private static async Task EnsureArticlesAsync(ApplicationDbContext db, string authorId)
        {
            await db.Articles.AddAsync(new Article(
                "What is Lorem Ipsum?",
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                authorId));

            await db.Articles.AddAsync(new Article(
                "Why do we use it?",
                "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                authorId));

            await db.Articles.AddAsync(new Article(
                "Where does it come from?",
                "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of 'de Finibus Bonorum et Malorum' (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, 'Lorem ipsum dolor sit amet..', comes from a line in section 1.10.32.",
                authorId));

            await db.Articles.AddAsync(new Article(
                "Where can I get some?",
                "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                authorId));

            await db.SaveChangesAsync();
        }
    }
}
