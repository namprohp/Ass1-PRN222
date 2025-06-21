using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VuHaiNamMVC.Models;
using VuHaiNamMVC.Services;

namespace VuHaiNamMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AccountController(IAccountService accountService, IAuthService authService)
        {
            _accountService = accountService;
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _accountService.Authenticate(model.Email, model.Password);
            if (user != null)
            {
                string role = user.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    3 => "Admin",
                    _ => "User"
                };

                await _authService.SignInUser(user.AccountEmail, user.AccountName, role, user.AccountId.ToString());

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }


        private async Task SignInUser(string email, string name, string role, string accountId)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, accountId),
        new Claim(ClaimTypes.Name, name),
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, role)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            ViewBag.ErrorMessage = "You do not have permission to access this page.";
            return View();
        }


    }
}
