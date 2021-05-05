using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CatalogServiceAPI.Services.CatalogService
{
    public interface ICatalogService
    {
        public Task<JsonResult> GetProductDataAsync(string location);
    }
}
