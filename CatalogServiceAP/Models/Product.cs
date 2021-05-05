using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogServiceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ProductCategory Category { get; set; }
        public int ProductCategoryId { get; set; }

        public List<ProductImage> Images { get; set; }
        public double Price { get; set; }
        public Shop Shop { get; set; }
        public int ShopId { get; set; }

    }
}
