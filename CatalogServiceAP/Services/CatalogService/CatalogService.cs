using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CatalogServiceAPI.Data;
using CatalogServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CatalogServiceAPI.Services.CatalogService
{
    public class CatalogService : ICatalogService
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly CatalogContext _catalogContext;
        private readonly ICatalogStorage _catalogStorage;

        /// <summary>
        /// Represent set of shops with its item id
        /// </summary>
        public sealed class ShopGlobalData
        {
            public sealed class ProductDetails
            {
                public int ProductId { get; set; }
                public string ProductName { get; set; }
                public string CategoryName { get; set; }
                public List<int> ImageIdList { get; set; }
                public double Price { get; set; }
            }

            public int ShopId { get; set; }
            public List<ProductDetails> ProductList { get; set; }

            public string Description { get; set; }
            public double Rate { get; set; }
        }

        public CatalogService(
            IConfiguration config,IMemoryCache cache,
            CatalogContext catalogContext, ICatalogStorage catalogStorage)
        {
            _config = config;
            _catalogContext = catalogContext;
            _cache = cache;
            _catalogStorage = catalogStorage;
        }

        public Task<JsonResult> GetProductDataAsync(string location)
        {
            return Task.Run(() => GetProductData(location));
        }

        private async Task<JsonResult> GetProductData(string location)
        {
            var availableShops = GetShopList(location);
            var shopDetailsList = new List<ShopGlobalData>();
            foreach (var shop in availableShops)
            {
                var products = await _catalogContext.Products
                    .Include(x => x.Images)
                    .Include(x => x.Category)
                    .Where(x => x.ShopId == shop.Id).ToListAsync();

                var details = new List<ShopGlobalData.ProductDetails>();

                foreach (var product in products)
                {
                    details.Add(new ShopGlobalData.ProductDetails()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        CategoryName = product.Category.Name,
                        ImageIdList = product.Images.Select(x => x.Id).ToList(),
                        Price = product.Price
                    });    
                }

                shopDetailsList.Add(new ShopGlobalData()
                {
                    ShopId = shop.Id,
                    Description = shop.Description,
                    ProductList = details,
                    Rate = shop.Rate
                });
            }


            return new JsonResult(shopDetailsList);
        }

        private List<Shop> GetShopList(string location)
        {
            var locationParts = location.Split(":");
            return _catalogStorage.GetShopList(locationParts[0], locationParts[1], locationParts[2]);
        }
    }
}
