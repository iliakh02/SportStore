using SportStore.Models.Entities;
using System.Collections.Generic;

namespace SportStore.WebUI.Models
{
    public class UsersViewModel
    {
        public List<User> Users { get; set; }
        public List<string> Roles { get; set; }
        public UsersSortViewModel SortViewModel { get; set; }
        public PageViewModel PageModel { get; set; }
    }
}
