using System.ComponentModel.DataAnnotations;

namespace SportStore.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please, enter login.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Please, enter password.")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
