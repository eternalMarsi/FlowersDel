using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogServiceAPI.Models.StorageModels
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public List<Shop> Shops;
    }
}
