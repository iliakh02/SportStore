using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Entities
{
    public class ProductOrder : IEntityBase
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
