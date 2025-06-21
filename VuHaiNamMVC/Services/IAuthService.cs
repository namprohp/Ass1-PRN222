using System.Threading.Tasks;

namespace VuHaiNamMVC.Services
{
    public interface IAuthService
    {
        Task SignInUser(string email, string name, string role, string accountId);
    }
}
