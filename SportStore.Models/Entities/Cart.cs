using System;
using System.Collections.Generic;
using System.Text;

namespace SportStore.Models.Entities
{
    public class Cart : IEntityBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<ProductCart> ProductCarts { get; set; }
    }
}
