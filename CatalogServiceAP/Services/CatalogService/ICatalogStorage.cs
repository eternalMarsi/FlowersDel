using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogServiceAPI.Models;

namespace CatalogServiceAPI.Services.CatalogService
{
    public interface ICatalogStorage
    {
        public void UpdateStorage();
        public List<Shop> GetShopList(string country, string region, string city);
    }
}
