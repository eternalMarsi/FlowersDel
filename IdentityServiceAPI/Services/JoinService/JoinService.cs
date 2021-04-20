using System;
using System.Threading.Tasks;
using IdentityServiceAPI.Data;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Models.ServiceResults;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceAPI.Services.JoinService
{
    public class JoinService : IJoinService
    {
        private readonly UserContext _userDb;
        private readonly ICryptoService _cryptoService;
        public JoinService(UserContext userDb , ICryptoService cryptoService)
        {
            _userDb = userDb;
            _cryptoService = cryptoService;
        }
        public Task<JoinResult> JoinUserAsync(RegistrationModel model)
        {
            return Task.Run(() => JoinUser(model));
        }

        private async Task<JoinResult> JoinUser(RegistrationModel model)
        {
            if(await _userDb.Users.Include(x => x.Role).AnyAsync(x => x.Username == model.Username))
            {
                return new JoinResult("Name is taken");
            }

            try
            {
                Role userRole;
                if (model.Token != String.Empty)
                {
                    var roleId = _cryptoService.VerifyToken(model.Token,
                        String.Format("{0} {1)", model.FirstName, model.LastName));
                    userRole = new Role(roleId);
                }
                else userRole = new Role(1);

                var localHashBytes = _cryptoService.GetLocalSaltBytes();
                var passwordBytes = _cryptoService.GetPasswordHash(model.PasswordString, localHashBytes);
                var newUser = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Username = model.Username,
                    Email = model.Email,
                    RoleId = userRole.Id,
                    LocalHashBytes = localHashBytes,
                    PasswordBytes = passwordBytes
                };

                _userDb.Users.Add(newUser);
                await _userDb.SaveChangesAsync();

                return new JoinResult()
                {
                    IsSuccessful = true,
                    User = newUser
                };
            }
            catch (DbUpdateException e)
            {
                return new JoinResult("Join error");
            }
            
        }
    }
}
