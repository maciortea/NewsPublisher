using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure
{
    public sealed class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string publisherUserId = await EnsureUserAsync(userManager, "publisher@test.com", "P@ssword1");
            string userId = await EnsureUserAsync(userManager, "test@test.com", "P@ssword1");
            await EnsureRoleAsync(roleManager, "User");
            await EnsureRoleAsync(roleManager, "Publisher");
            await AssignRolesToUserAsync(userManager, userId, "User");
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
                "What are some reasons programmers quit their high paying jobs?",
                @"I was getting in excess of $120,000 when i quit my job last May. I was working in San Jose and had an apartment and a sports car.
                  Though i always knew i will quit, here are reasons which led my quitting materialize:
                  Monotonous work.
                  I realized that i am a very small player in a large eco - system of the product = lesser importance.
                  I was always interested in starting a business.I did an M.S only with the flow.
                  I quit my job and solo - travelled 10 countries.
                  I started working in my family business in India since then.",
                authorId));

            await db.Articles.AddAsync(new Article(
                "What is the single biggest green flag during a job interview?",
                @"You’ll sit down in your interview. You’ll be on your best behavior.
                  He will have your resume in front of him. He will start “sizing you up”. Asking about prior roles, skills, stupid HR questions that are designed to make you squirm. “What was your biggest mistake and how did you fix it?”
                  (Hint: leave out that first one that comes to mind. That one is typically a bit too raw :P)
                  He’ll continue with his barrage of questions. Trying to read between the lines on your bullet points.
                  And then.
                  Somewhere in the middle of the interview.
                  The tone will change.
                  Rather than sizing you up, he will start selling you up on the job.
                  “This job has great perks….great coworkers….work life balance…”
                  It is usually a great sign. Don’t act too excited though. Play hard to get a little. Just say, “I have a few interviews to go on this week. I’ve just begun the process of my search.”
                  Now, before you get too excited if you see this, remember: Interviewing is its own dystopian dating hell of sorts. This “excitement” can happen and you can get ghosted right after that.
                  The problem is that they are required to interview X number of candidates. And it’s entirely possible another great candidate comes in after that and steals your thunder. And another. And another.
                  So keep your head up. Just remember: you only have to find one job.",
                authorId));

            await db.SaveChangesAsync();
        }
    }
}
