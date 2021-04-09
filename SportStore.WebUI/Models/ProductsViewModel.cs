using Microsoft.AspNetCore.Mvc.Rendering;
using SportStore.Models.Entities;
using System.Collections.Generic;

namespace SportStore.WebUI.Models
{
    public class ProductsViewModel
    {
        public List<Product> Products { get; set; }
        public ProductsSortViewModel SortViewModel { get; set; }
        public PageViewModel PageModel { get; set; }
        public string SearchString { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string CurrentCategory { get; set; }
    }
}
