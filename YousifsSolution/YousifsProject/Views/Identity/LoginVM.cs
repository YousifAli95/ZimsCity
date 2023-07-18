using System.ComponentModel.DataAnnotations;

namespace YousifsProject.Views.Identity
{
    public class LoginVM
    {
        [EmailAddress]
        [StringLength(maximumLength: 100)]
        [Display(Name = "Username")]
        [Required]
        public string? Username { get; set; }

        [StringLength(maximumLength: 100)]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }

}

