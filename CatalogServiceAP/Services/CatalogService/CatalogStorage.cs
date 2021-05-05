using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CatalogServiceAPI.Data;
using CatalogServiceAPI.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CatalogServiceAPI.Services.CatalogService
{
    public class CatalogStorage : ICatalogStorage
    {

        private readonly IConfigurationRoot _serviceSettings;
        private readonly CatalogContext _catalogContext;
        private readonly IMemoryCache _cache;


        private readonly int _minShopCount;
        private readonly int _cacheLifetime;

        public CatalogStorage(IConfiguration config, IMemoryCache cache, CatalogContext catalogContext)
        {
            _cache = cache;
            _catalogContext = catalogContext;

            var settingsPath = config["ServiceSettingsPath"];
            _serviceSettings = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(settingsPath)
                .Build();


            _minShopCount = Int32.Parse(_serviceSettings["MinShopCount"]);
            _cacheLifetime = Int32.Parse(_serviceSettings["CacheLifetime"]);
        }

        public void UpdateStorage()
        {
            var cachedCities = _catalogContext.Cities.Include(x => x.Shops).Where(x => x.Shops.Count >= _minShopCount);

            foreach (var city in cachedCities)
            {
                var country = city.Country;
                var region = city.Region;
                var cityShops = new List<Shop>();

                cityShops.AddRange(
                    _catalogContext.Shops.Where(x => city.Shops.Contains(x))
                );

                var cacheKey = CreateCacheKey(country, region, city.CityName);
                _cache.Set(cacheKey, cityShops,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_cacheLifetime)));
            }
        }


        public List<Shop> GetShopList(string country, string region, string city)
        {
            var cacheKey = CreateCacheKey(country, region, city);
            if (_cache.TryGetValue(cacheKey, out List<Shop> shops))
            {
                return shops;
            }
            else
            {
                UpdateStorage();
                return GetShopList(country, region, city);
            }
        }

        private string CreateCacheKey(string country, string region, string city) => String.Format("{0}:{1}:{2}", country, region, city);
    }
}
