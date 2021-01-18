using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsProject.Areas.Identity.Data;
using NewsProject.Data;
using NewsProject.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NewsProject.Interfaces;

namespace NewsProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileService _fileService;
        private readonly INewsRepository _newsRepository;
        public HomeController(ILogger<HomeController> logger, NewsDbContext context, 
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileService fileService, INewsRepository newsRepository)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
            _newsRepository = newsRepository;
        }

        public async Task<ActionResult<IEnumerable<News>>> Index()
        {
            var news = await _newsRepository.GetNewsAsync(); 
            
            return View(news);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news)
        {
            if (ModelState.IsValid)
            {
                news.AuthorId = _userManager.GetUserId(User);

                if (await _newsRepository.SaveAllAsync(news)) {

                    _fileService.AddNewsJsonFile(news);

                    return RedirectToAction("Index");
                }  
            }
            return View(news);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(News news)
        {

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var newsFromDatabase = await _newsRepository.GetNewsByIdAsync(news.Id);
               
                news.AuthorId = newsFromDatabase.AuthorId;
                if (await _newsRepository.UpdateNews(news, userId))
                {
                    _fileService.UpdateNewsJsonFile(news);

                }
                else
                {
                    TempData["error"] = "* Only user owner can edit this news";
                    return View("Edit", news);
                }
                return RedirectToAction("Index");
            }
            return View(news);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var userId = _userManager.GetUserId(User);
            
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
               
            if (await _newsRepository.DeleteNews(news, userId))
            {
                _fileService.DeleteNewsJsonFile(news);
            }
            else
            {
                TempData["error"] = "* Only user owner can delete this news";
                return View("Delete", news);
            }
            return RedirectToAction("Index");
        
        }
        public async Task<IActionResult> ViewNews(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var news = await _newsRepository.GetNewsByIdAsync(id);

            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
