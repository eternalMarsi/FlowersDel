using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using IdentityServiceAPI.Services.JoinService;

namespace IdentityServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IJoinService _joinService;
        private readonly ICryptoService _cryptoService;
        public RegistrationController(IJoinService joinService, ICryptoService cryptoService)
        {
            _joinService = joinService;
            _cryptoService = cryptoService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(RegistrationModel model)
        {
            var result = await _joinService.JoinUserAsync(model);

            if (result.IsSuccessful)
            {
                var token = _cryptoService.CreateToken(result.User);

                var response = new
                {
                    token = token,
                    username = result.User.Username
                };

                return new JsonResult(response);
            }

            return new JsonResult(result.ErrorMessage);
        }
    }
}
