using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogServiceAPI.Models
{
    public class ShopImage
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }

        public Shop Shop { get; set; }
        public int ShopId { get; set; }
    }
}
