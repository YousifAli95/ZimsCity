using System.ComponentModel.DataAnnotations;

namespace YousifsProject.Views.Identity.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

}

