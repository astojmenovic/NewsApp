using NewsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsProject.Interfaces
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetNewsAsync();
        Task<bool> UpdateNews(News news, string userId);
        Task<bool> DeleteNews(News news, string userId);
        Task<News> GetNewsByIdAsync(int? id);
        Task<bool> SaveAllAsync(News news);
    }
}
