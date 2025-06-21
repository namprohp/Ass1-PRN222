using Microsoft.EntityFrameworkCore;
using VuHaiNamMVC.Models;

namespace VuHaiNamMVC.Services
{
    public class AccountService : IAccountService
    {
        private readonly FunewsManagementContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(FunewsManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public SystemAccount Authenticate(string email, string password)
        {
            // Kiểm tra tài khoản admin mặc định từ appsettings.json
            var adminEmail = _configuration["DefaultAdmin:Email"];
            var adminPassword = _configuration["DefaultAdmin:Password"];
            if (email == adminEmail && password == adminPassword)
            {
                return new SystemAccount
                {
                    AccountId = 0,
                    AccountEmail = adminEmail,
                    AccountRole = 3, // 3 = Admin
                    AccountName = "Administrator"
                };
            }

            // Kiểm tra tài khoản từ database
            return _context.SystemAccounts
                .FirstOrDefault(u => u.AccountEmail == email && u.AccountPassword == password);
        }
    }
}
