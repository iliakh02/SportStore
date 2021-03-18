using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please, enter name.")]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please, enter surname.")]
        [MaxLength(100)]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please, enter phone number.")]
        [MaxLength(13)]
        [MinLength(13)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please, enter email.")]
        [MaxLength(100)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, enter password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please, enter password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are mismatch.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
