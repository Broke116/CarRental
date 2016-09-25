using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email address required")]
        [EmailAddress(ErrorMessage = "Must be valid address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}