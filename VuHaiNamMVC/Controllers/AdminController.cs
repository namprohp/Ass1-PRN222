using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VuHaiNamMVC.Models;

namespace VuHaiNamMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly FunewsManagementContext _context;

        public AdminController(FunewsManagementContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị danh sách tài khoản + tìm kiếm
        public async Task<IActionResult> ManageAccount(string search)
        {
            var accounts = _context.SystemAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                accounts = accounts.Where(a => a.AccountName.Contains(search) || a.AccountEmail.Contains(search));
            }

            return View(await accounts.ToListAsync());
        }

        // ✅ Hiển thị trang tạo tài khoản
        public IActionResult Create()
        {
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecturer" }
            };
            return View(new SystemAccount());
        }

        // ✅ Xử lý tạo tài khoản
        [HttpPost]
        public async Task<IActionResult> Create(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                var maxId = _context.SystemAccounts.Max(a => (short?)a.AccountId) ?? 0;
                account.AccountId = (short)(maxId + 1);
                _context.SystemAccounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageAccount)); // Quay lại danh sách
            }
            return View(account);
        }

        public async Task<IActionResult> Edit(short id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            ViewBag.Roles = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Staff" },
        new SelectListItem { Value = "2", Text = "Lecturer" }
    };

            return View(account);
        }


        // ✅ Xử lý cập nhật tài khoản
        [HttpPost]
        public async Task<IActionResult> Edit(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageAccount)); // Quay lại danh sách
            }
            return View(account);
        }

        public async Task<IActionResult> Delete(short id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found.";
                return RedirectToAction(nameof(ManageAccount));
            }

            // ❌ Không cho phép xóa Admin
            if (account.AccountRole == 3)
            {
                TempData["ErrorMessage"] = "Admin accounts cannot be deleted.";
                return RedirectToAction(nameof(ManageAccount));
            }

            try
            {
                _context.SystemAccounts.Remove(account);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Account deleted successfully.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to delete account. It may be referenced in another table.";
            }

            return RedirectToAction(nameof(ManageAccount));
        }

        public async Task<IActionResult> ReportStatistics(DateTime? startDate, DateTime? endDate)
        {
            var newsArticles = _context.NewsArticles.AsQueryable();

            if (startDate.HasValue)
            {
                newsArticles = newsArticles.Where(n => n.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                newsArticles = newsArticles.Where(n => n.CreatedDate <= endDate.Value);
            }

            // ✅ Sắp xếp theo CreatedDate giảm dần
            newsArticles = newsArticles.OrderByDescending(n => n.CreatedDate);

            return View(await newsArticles.ToListAsync());
        }

        // ✅ Chức năng duyệt bài viết (Accept bài viết theo Category)
        [HttpPost]
        public async Task<IActionResult> UpdateCategoryStatus(short categoryId, bool isActive)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            category.IsActive = isActive;
            _context.Update(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category status updated successfully.";
            return RedirectToAction(nameof(ManageCategory));
        }

        // ✅ Quản lý danh sách Category
        public async Task<IActionResult> ManageCategory()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> ManageNews()
        {
            var newsArticles = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Select(n => new
                {
                    n.NewsArticleId,
                    n.NewsTitle,
                    n.CreatedDate,
                    CategoryName = n.Category != null ? n.Category.CategoryName : "N/A",
                    CreatedByName = n.CreatedBy != null ? n.CreatedBy.AccountName : "N/A",
                    UpdatedByName = _context.SystemAccounts
                        .Where(s => s.AccountId == n.UpdatedById)
                        .Select(s => s.AccountName)
                        .FirstOrDefault(), // ✅ Lấy tên UpdatedBy từ DB
                    n.ModifiedDate,
                    n.NewsStatus
                })
                .ToListAsync();

            return View(newsArticles);
        }

        public async Task<IActionResult> ViewNews(string id)
        {
            var article = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Select(n => new
                {
                    n.NewsArticleId,
                    n.NewsTitle,
                    n.Headline,
                    n.CreatedDate,
                    n.NewsContent,
                    n.NewsSource,
                    CategoryName = n.Category != null ? n.Category.CategoryName : "N/A",
                    CreatedByName = n.CreatedBy != null ? n.CreatedBy.AccountName : "N/A",
                    UpdatedByName = _context.SystemAccounts
                        .Where(s => s.AccountId == n.UpdatedById)
                        .Select(s => s.AccountName)
                        .FirstOrDefault(),
                    n.ModifiedDate
                })
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // ✅ Cập nhật trạng thái duyệt bài viết (Accept / Reject)
        [HttpPost]
        public async Task<IActionResult> UpdateNewsStatus(string id, bool newsStatus)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            article.NewsStatus = newsStatus;
            _context.Update(article);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "News status updated successfully.";
            return RedirectToAction(nameof(ManageNews));
        }

    }
}
