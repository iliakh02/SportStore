using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Models
{
    public class OrderCreateViewModel
    {
        public List<CartItem> Cart { get; set; }
        public User User { get; set; }
    }
}
