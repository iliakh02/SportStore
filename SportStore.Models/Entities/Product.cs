using System;
using System.Collections.Generic;
using System.Text;

namespace SportStore.Models.Entities
{
    public class Product : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public double Discount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Description { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }

        public ICollection<Cart> Carts { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
