﻿using ApplicationCore.Entities.ArticleAggregate;
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
            builder.Entity<ArticleLike>(ConfigureArticleLike);
            builder.Entity<Comment>(ConfigureComment);
        }

        private void ConfigureArticle(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Body).IsRequired();
            builder.Property(a => a.AuthorId).IsRequired();

            var likesNavigation = builder.Metadata.FindNavigation(nameof(Article.Likes));
            likesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var commentsNavigation = builder.Metadata.FindNavigation(nameof(Article.Comments));
            commentsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureArticleLike(EntityTypeBuilder<ArticleLike> builder)
        {
            builder.HasKey(al => al.Id);
            builder.HasOne(al => al.Article).WithMany(a => a.Likes);
        }

        private void ConfigureComment(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Text).HasMaxLength(500).IsRequired();
            builder.HasOne(c => c.Article).WithMany(a => a.Comments);
        }
    }
}
