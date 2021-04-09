using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Entities
{
    public class CartItem : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public int Amount { get; set; }

    }
}
