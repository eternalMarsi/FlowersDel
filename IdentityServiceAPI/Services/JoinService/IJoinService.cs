using System.Threading.Tasks;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Models.ServiceResults;

namespace IdentityServiceAPI.Services.JoinService
{
    public interface IJoinService
    {
        public Task<JoinResult> JoinUserAsync(RegistrationModel model);
    }
}
