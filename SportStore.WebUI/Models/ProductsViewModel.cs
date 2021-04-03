using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Models
{
    public class ProductsViewModel
    {
        public List<Product> Products { get; set; }
        public ProductsSortViewModel SortViewModel { get; set; }
        public PageViewModel PageModel { get; set; }
        public string SearchString { get; set; }
    }
}
