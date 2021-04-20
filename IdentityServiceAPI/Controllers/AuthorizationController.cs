using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using IdentityServiceAPI.Services.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICryptoService _cryptoService;
        public AuthorizationController(IAuthService authService, ICryptoService cryptoService)
        {
            _authService = authService;
            _cryptoService = cryptoService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(AuthModel model)
        {
            var user = await _authService.ValidateUserAsync(model);

            if (user == null)
            {
                return NotFound();
            }

            var token = _cryptoService.CreateToken(user);

            var response = new
            {
                token = token,
                username = user.Username
            };

            return new JsonResult(response);
        }
    }
}
