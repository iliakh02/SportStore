using Microsoft.AspNetCore.Identity;
using SportStore.Models.Entities;
using System.Collections.Generic;

namespace SportStore.WebUI.Models
{
    public class UserEditViewModel
    {
        public User User { get; set; }
        public List<IdentityRole<int>> AllRoles { get; set; }
        public List<string> ActiveRoles { get; set; }
    }
}
