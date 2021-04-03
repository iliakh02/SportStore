using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Models
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name can't be null.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Producer can't be null.")]
        public string Producer { get; set; }
        [Required(ErrorMessage = "Discount can't be null.")]
        [Range(0, 1)]
        public double Discount { get; set; }

        [Required(ErrorMessage = "Category can't be null.")]
        public int CategoryId { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "Discount can't be null.")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Prise can't be null.")]
        public decimal Price { get; set; }

        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
