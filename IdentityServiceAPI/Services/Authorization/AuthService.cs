using System.Threading.Tasks;
using IdentityServiceAPI.Data;
using IdentityServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceAPI.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly UserContext _userDb;
        private readonly ICryptoService _cryptoService;
        public AuthService(UserContext userDb, ICryptoService cryptoService)
        {
            _userDb = userDb;
            _cryptoService = cryptoService;

        }
        

        public Task<User> ValidateUserAsync(AuthModel model)
        {
            return Task.Run(() => ValidateUser(model));
        }

        private async Task<User> ValidateUser(AuthModel model)
        {
            var user = await _userDb.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Username == model.Username);

            //if user was not found
            if (user == null)
            {
                return null;
            }
                
            //if password is incorrect
            if (! await _cryptoService.VerifyPasswordAsync(user.PasswordBytes, model.PasswordString,
                user.LocalHashBytes))
            {
                return null;
            }

            return user;

        }



    }
}
