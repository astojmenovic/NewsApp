using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsProject.Data;
using NewsProject.Interfaces;
using NewsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsProject.Areas.Identity.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public NewsRepository(NewsDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<News>> GetNewsAsync()
        {
            return await _context.News.Include(u => u.Author).ToListAsync();
        }

        public async Task<News> GetNewsByIdAsync(int? id)
        {
            return await _context.News.AsNoTracking().Include(u => u.Author).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAllAsync(News news)
        {
            _context.News.Add(news);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateNews(News news, string userId)
        {
            
            if (userId == news.AuthorId)
            {
                _context.News.Update(news);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
            
        }

        public async Task<bool> DeleteNews(News news, string userId)
        {
            if(userId == news.AuthorId)
            {
                _context.News.Remove(news);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
            
        }
    }
}
