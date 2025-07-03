using Data.Entitys;
using System.Threading.Tasks;

namespace Bussiness.Interfaces
{
    public interface IUserService
    {
        Task<User?> LoginAsync(string userEmail, string password);
        Task<bool> RegisterAsync(User user, string password);
    }
}
