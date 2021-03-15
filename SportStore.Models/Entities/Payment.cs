using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Models.Entities
{
    public class Payment : IEntityBase
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
