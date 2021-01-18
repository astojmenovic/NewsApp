using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsProject.Areas.Identity.Data;
using NewsProject.Data;

[assembly: HostingStartup(typeof(NewsProject.Areas.Identity.IdentityHostingStartup))]
namespace NewsProject.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NewsDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("NewsDbContextConnection")));

                services.AddDefaultIdentity<AppUser>(options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<NewsDbContext>();
            });
        }
    }
}