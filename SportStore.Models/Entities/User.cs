using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Entities
{
    public class User : IdentityUser<int>
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<CartItem> Cart { get; set; }
    }
}
