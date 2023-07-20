using System.ComponentModel.DataAnnotations;

namespace YousifsProject.Views.Identity
{
    public class LoginVM
    {
        [StringLength(maximumLength: 100, MinimumLength = 6, ErrorMessage = "Your Username must be between 6 and 100 characters long.")]
        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 6, ErrorMessage = "Your Password must be between 6 and 100 characters long.")]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

}

