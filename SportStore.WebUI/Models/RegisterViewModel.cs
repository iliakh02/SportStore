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
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please, enter last name.")]
        [MaxLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please, enter username.")]
        [MaxLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please, enter phone number.")]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^((8|\+)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "This phone number is not valid.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please, enter email.")]
        [MaxLength(100)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, enter password.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*():;><№~+=, -]).{8,}$", 
            ErrorMessage = "This password is not valid.\r\nPassword has contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please, enter password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are mismatch.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*():;><№~+=, -]).{8,}$", 
            ErrorMessage = "This password is not valid.\r\nPassword has contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
