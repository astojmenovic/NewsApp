using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsProject.Areas.Identity.Data;
using NewsProject.Models;

namespace NewsProject.Data
{
    public class NewsDbContext : IdentityDbContext<AppUser>
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(u => u.News)
                .WithOne(n => n.Author)
                .HasForeignKey(ur => ur.AuthorId);
        }
        public DbSet<News> News { get; set; }
    }
}
