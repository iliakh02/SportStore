using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Models
{
    public class CategoriesViewModel
    {
        public List<Category> Categories { get; set; }
        public CategoriesSortViewModel SortViewModel { get; set; }
        public PageViewModel PageModel { get; set; }
    }
}
