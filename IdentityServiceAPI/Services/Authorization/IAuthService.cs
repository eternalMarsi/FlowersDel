using System.Threading.Tasks;
using IdentityServiceAPI.Models;

namespace IdentityServiceAPI.Services.Authorization
{
    public interface IAuthService
    {
        public Task<User> ValidateUserAsync(AuthModel model);
    }
}
