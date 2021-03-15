using System;
using System.Collections.Generic;
using System.Text;

namespace SportStore.Models.Entities
{
    public class Order : IEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public bool Paid { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
