using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogServiceAPI.Services.CatalogService;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        public ShopController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Products(string location)
        {
            var jsonResult = await _catalogService.GetProductDataAsync(location);

            if (jsonResult != null)
                return jsonResult;
            return new NotFoundResult();
        }
    }
}
