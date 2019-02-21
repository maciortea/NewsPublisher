using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface INewsRepository
    {
        Task<IReadOnlyList<Article>> GetAllAsync();
        Task<IReadOnlyList<Article>> GetAllByAuthorIdAsync(string authorId);
        Task AddAsync(Article article);
        Task<Article> GetByIdAsync(int id);
    }
}
