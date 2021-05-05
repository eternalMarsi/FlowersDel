using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogServiceAPI.Models;
using CatalogServiceAPI.Models.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Data
{
    public class CatalogContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopImage> ShopImages { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<City> Cities { get; set; }
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }
    }
}
