using VuHaiNamMVC.Models;

namespace VuHaiNamMVC.Services
{
    public interface IAccountService
    {
        SystemAccount Authenticate(string email, string password);
    }
}
