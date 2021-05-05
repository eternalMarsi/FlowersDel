using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogServiceAPI.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
