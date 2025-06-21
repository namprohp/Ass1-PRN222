using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VuHaiNamMVC.Models;

namespace VuHaiNamMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly FunewsManagementContext _context;

        public HomeController(ILogger<HomeController> logger, FunewsManagementContext context)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index()
        {
            var newsArticles = await _context.NewsArticles
                .Where(n => n.NewsStatus == true) // Chỉ lấy tin đã được duyệt
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new
                {
                    n.NewsArticleId,
                    n.NewsTitle,
                    n.Headline,
                    n.CreatedDate,
                    CategoryName = n.Category != null ? n.Category.CategoryName : "N/A",
                    CreatedByName = n.CreatedBy != null ? n.CreatedBy.AccountName : "N/A"
                })
                .ToListAsync();

            return View(newsArticles);
        }

        public async Task<IActionResult> ViewNews(string id)
        {
            var article = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
