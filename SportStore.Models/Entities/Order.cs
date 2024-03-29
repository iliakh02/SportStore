﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Entities
{
    public class Order : IEntityBase
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public bool Paid { get; set; }
        public List<ProductOrder> ProductOrders { get; set; }
    }
}
