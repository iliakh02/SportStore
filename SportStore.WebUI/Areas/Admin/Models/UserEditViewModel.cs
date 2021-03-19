using Microsoft.AspNetCore.Identity;
using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Areas.Admin.Models
{
    public class UserEditViewModel
    {
        public User User { get; set; }
        public List<IdentityRole<int>> AllRoles { get; set; }
        public List<string> ActiveRoles { get; set; }
    }
}
