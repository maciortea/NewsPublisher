using ApplicationCore.Entities.ArticleAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface INewsRepository
    {
        Task<IReadOnlyList<Article>> GetAllAsync();
        Task<IReadOnlyList<Article>> GetAllByAuthorIdAsync(string authorId);
        Task<Article> GetByIdAsync(int id);
        Task AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task DeleteAsync(Article article);
    }
}
