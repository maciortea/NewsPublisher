using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Article> Articles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>(ConfigureArticle);
        }

        private void ConfigureArticle(EntityTypeBuilder<Article> builder)
        {
            builder.Property(a => a.Title).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Body).IsRequired();
            builder.Property(a => a.AuthorId).IsRequired();
        }
    }
}
