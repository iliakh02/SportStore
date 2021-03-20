using SportStore.Models.Entities;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Areas.Admin.Models
{
    public class UsersViewModel
    {
        public List<User> Users { get; set; }
        public List<string> Roles { get; set; }
        public UsersSortViewModel SortViewModel { get; set; }
        public PageViewModel PageModel { get; set; }
    }
}
