using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Models.Entities
{
    public class Payment : IEntityBase
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Type { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
