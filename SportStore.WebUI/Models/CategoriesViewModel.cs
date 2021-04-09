using SportStore.Models.Entities;
using System.Collections.Generic;

namespace SportStore.WebUI.Models
{
    public class CategoriesViewModel
    {
        public List<Category> Categories { get; set; }
        public CategoriesSortViewModel SortViewModel { get; set; }
        public PageViewModel PageModel { get; set; }
        public string SearchString { get; set; }
    }
}
