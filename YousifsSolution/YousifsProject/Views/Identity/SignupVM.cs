using System.ComponentModel.DataAnnotations;

namespace YousifsProject.Views.Identity
{
    public class SignupVM
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

        [StringLength(maximumLength: 100)]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
