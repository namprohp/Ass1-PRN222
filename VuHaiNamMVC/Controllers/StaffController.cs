using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VuHaiNamMVC.Models;
using VuHaiNamMVC.Services;

namespace VuHaiNamMVC.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly FunewsManagementContext _context;

        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public StaffController(FunewsManagementContext context, IAuthService authService, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            _configuration = configuration;
        }


        // ✅ Quản lý Category
        public async Task<IActionResult> ManageCategory(string search)
        {
            var categories = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(c => c.CategoryName.Contains(search));
            }

            var categoryList = await categories.OrderBy(c => c.CategoryName).ToListAsync();
            return View(categoryList);
        }

        // ✅ Tạo mới Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category added successfully.";
            }
            return RedirectToAction(nameof(ManageCategory));
        }

        // ✅ Chỉnh sửa Category
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category updated successfully.";
            }
            return RedirectToAction(nameof(ManageCategory));
        }

        // ✅ Kiểm tra trước khi xóa Category
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found.";
                return RedirectToAction(nameof(ManageCategory));
            }

            // Kiểm tra nếu Category có bài viết thì không thể xóa
            bool hasNewsArticles = _context.NewsArticles.Any(n => n.CategoryId == id);
            if (hasNewsArticles)
            {
                TempData["ErrorMessage"] = "Cannot delete this category because it is assigned to news articles.";
                return RedirectToAction(nameof(ManageCategory));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Category deleted successfully.";
            return RedirectToAction(nameof(ManageCategory));
        }

        public async Task<IActionResult> ManageNewsArticle(string search)
        {
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.SystemAccounts = await _context.SystemAccounts.OrderBy(s => s.AccountName).ToListAsync();

            var newsArticles = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Select(n => new
                {
                    n.NewsArticleId,
                    n.NewsTitle,
                    n.Headline,
                    n.CreatedDate,
                    CategoryName = n.Category != null ? n.Category.CategoryName : "N/A",
                    CreatedByName = n.CreatedBy != null ? n.CreatedBy.AccountName : "N/A",
                    UpdatedByName = _context.SystemAccounts
                        .Where(s => s.AccountId == n.UpdatedById)
                        .Select(s => s.AccountName)
                        .FirstOrDefault(),
                    n.ModifiedDate,
                    n.NewsStatus
                })
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return View(newsArticles);
        }

        // ✅ Xem chi tiết bài viết
        public async Task<IActionResult> ViewNewsArticle(string id)
        {
            var article = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            ViewBag.UpdatedByName = _context.SystemAccounts
                .Where(s => s.AccountId == article.UpdatedById)
                .Select(s => s.AccountName)
                .FirstOrDefault();

            return View(article);
        }


        [HttpGet]
        public async Task<IActionResult> CreateNewsArticle()
        {
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.SystemAccounts = await _context.SystemAccounts.OrderBy(s => s.AccountName).ToListAsync();

            var newsArticle = new NewsArticle
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                NewsStatus = false // Mặc định "Pending"
            };

            return View(newsArticle);
        }



        [HttpPost]
        public async Task<IActionResult> CreateNewsArticle(NewsArticle model)
        {
            if (ModelState.IsValid)
            {
                // ✅ Kiểm tra ID có bị trùng không
                var existingArticle = await _context.NewsArticles.FindAsync(model.NewsArticleId);
                if (existingArticle != null)
                {
                    TempData["Error"] = "❌ News Article ID already exists. Please choose another one.";
                    return RedirectToAction("CreateNewsArticle");
                }

                // ✅ Gán giá trị mặc định
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.NewsStatus = false; // Mặc định là "Pending"

                // ✅ Chuyển đổi ID từ View về `short`
                model.CategoryId = (short?)model.CategoryId;
                model.CreatedById = (short?)model.CreatedById;
                model.UpdatedById = string.IsNullOrEmpty(model.UpdatedById?.ToString()) ? null : (short?)model.UpdatedById;

                _context.NewsArticles.Add(model);
                await _context.SaveChangesAsync();

               

                TempData["Success"] = "✅ News article added successfully!";
                return RedirectToAction("ManageNewsArticle");
            }

            // Nếu lỗi, hiển thị lại form với danh sách danh mục và tài khoản
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SystemAccounts = await _context.SystemAccounts.ToListAsync();

            TempData["Error"] = "❌ Validation failed. Please check your inputs.";
            return View(model);
        }



        // 🟢 Hiển thị trang chỉnh sửa bài viết
        [HttpGet]
        public async Task<IActionResult> EditNewsArticle(string id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            // Load dữ liệu Categories & Users để hiển thị trong dropdown
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SystemAccounts = await _context.SystemAccounts.ToListAsync();

            return View(article);
        }

        // 🟢 Xử lý cập nhật bài viết
        [HttpPost]
        public async Task<IActionResult> EditNewsArticle(NewsArticle model)
        {
            if (ModelState.IsValid)
            {
                var existingArticle = await _context.NewsArticles.FindAsync(model.NewsArticleId);
                if (existingArticle == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin từ model
                existingArticle.NewsTitle = model.NewsTitle;
                existingArticle.Headline = model.Headline;
                existingArticle.NewsContent = model.NewsContent;
                existingArticle.NewsSource = model.NewsSource;
                existingArticle.CategoryId = model.CategoryId;
                existingArticle.UpdatedById = model.UpdatedById;
                existingArticle.ModifiedDate = DateTime.Now; // ✅ Cập nhật ngày sửa

                _context.NewsArticles.Update(existingArticle);
                await _context.SaveChangesAsync();

                TempData["Success"] = "News article updated successfully!";
                return RedirectToAction("ManageNewsArticle");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SystemAccounts = await _context.SystemAccounts.ToListAsync();
            return View(model);
        }

        // 🟢 Hiển thị trang xác nhận xóa
        [HttpGet]
        public async Task<IActionResult> DeleteNewsArticle(string id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [HttpPost, ActionName("DeleteNewsArticle")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "News article not found.";
                return RedirectToAction("ManageNewsArticle");
            }

            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "News article deleted successfully!";
            return RedirectToAction("ManageNewsArticle");
        }

        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            var staff = await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId.ToString() == userId);
            if (staff == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("Login", "Account");
            }

            return View(staff);
        }


        [HttpPost]
        public async Task<IActionResult> Profile(SystemAccount updatedProfile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.ErrorMessage = "Session expired. Please log in again.";
                return View(updatedProfile);
            }

            var staff = await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId.ToString() == userId);
            if (staff == null)
            {
                ViewBag.ErrorMessage = "User not found!";
                return View(updatedProfile);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Validation failed. Please check your inputs.";
                return View(updatedProfile);
            }

            // ✅ Cập nhật thông tin cá nhân
            staff.AccountName = updatedProfile.AccountName;
            staff.AccountEmail = updatedProfile.AccountEmail;

            await _context.SaveChangesAsync();

            // ✅ Cập nhật lại session để thay đổi tên ngay lập tức
            await UpdateUserSession(staff.AccountId, staff.AccountName, staff.AccountRole ?? 0);


            ViewBag.SuccessMessage = "Profile updated successfully!";
            return View(staff);
        }

        // ✅ Hàm cập nhật lại session
        private async Task UpdateUserSession(short accountId, string accountName, int role)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
        new Claim(ClaimTypes.Name, accountName),
        new Claim(ClaimTypes.Role, role == 1 ? "Staff" : "User")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.ErrorMessage = "Session expired. Please log in again.";
                return View("Profile");
            }

            var user = await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId.ToString() == userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found!";
                return View("Profile");
            }

            // ✅ Kiểm tra mật khẩu cũ
            if (user.AccountPassword != OldPassword)
            {
                ViewBag.ErrorMessage = "Incorrect old password!";
                return View("Profile", user);
            }

            // ✅ Kiểm tra mật khẩu mới và xác nhận mật khẩu
            if (NewPassword != ConfirmPassword)
            {
                ViewBag.ErrorMessage = "New passwords do not match!";
                return View("Profile", user);
            }

            // ✅ Cập nhật mật khẩu mới
            user.AccountPassword = NewPassword;
            await _context.SaveChangesAsync();

            // ✅ Hiển thị thông báo thành công trên trang Profile
            ViewBag.SuccessMessage = "Password changed successfully!";
            return View("Profile", user);
        }

        [HttpGet]
        public async Task<IActionResult> ViewNewsHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của user đăng nhập

            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.ErrorMessage = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            var newsHistory = await _context.NewsArticles
                .Where(n => n.CreatedById.ToString() == userId || n.UpdatedById.ToString() == userId)
                .OrderByDescending(n => n.ModifiedDate)
                .Select(n => new
                {
                    n.NewsArticleId,
                    n.NewsTitle,
                    n.Headline,
                    n.CreatedDate,
                    n.ModifiedDate,
                    CategoryName = n.Category != null ? n.Category.CategoryName : "N/A",
                    CreatedByName = n.CreatedBy != null ? n.CreatedBy.AccountName : "N/A",
                    UpdatedByName = _context.SystemAccounts
                        .Where(s => s.AccountId == n.UpdatedById)
                        .Select(s => s.AccountName)
                        .FirstOrDefault(),
                    n.NewsStatus
                })
                .ToListAsync();

            return View(newsHistory);
        }

        

    }
}
