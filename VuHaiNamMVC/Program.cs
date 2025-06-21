using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VuHaiNamMVC.Models;
using VuHaiNamMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Kết nối với Database
var connectionString = builder.Configuration.GetConnectionString("FUNewsManagementSystemDB");
builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Authentication với Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Trang đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied"; // Trang từ chối truy cập
    });

builder.Services.AddAuthorization();

// Đăng ký Service xử lý tài khoản
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor(); // ✅ Để sử dụng `IHttpContextAccessor`
builder.Services.AddScoped<IEmailService, EmailService>();

// Thêm MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình middleware
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
