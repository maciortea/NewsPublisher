﻿using ApplicationCore.Entities.ArticleAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NewsRepository : Repository, INewsRepository
    {
        public NewsRepository(ApplicationDbContext db)
            : base(db)
        {
        }

        public async Task<IReadOnlyList<Article>> GetAllAsync()
        {
            return await _db.Articles.OrderByDescending(a => a.PublishDate).ToListAsync();
        }

        public async Task<IReadOnlyList<Article>> GetAllByAuthorIdAsync(string authorId)
        {
            return await _db.Articles.Where(a => a.AuthorId == authorId).OrderByDescending(a => a.PublishDate).ToListAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _db.Articles.Include(a => a.Likes).Include(a => a.Comments).SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Article article)
        {
            await _db.Articles.AddAsync(article);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Article article)
        {
            _db.Entry(article).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Article article)
        {
            _db.Articles.Remove(article);
            await _db.SaveChangesAsync();
        }
    }
}
