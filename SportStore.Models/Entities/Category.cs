using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Entities
{
    public class Category : IEntityBase
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
