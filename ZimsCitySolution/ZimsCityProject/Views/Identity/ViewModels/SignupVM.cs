﻿using System.ComponentModel.DataAnnotations;

namespace ZimsCityProject.Views.Identity.ViewModels
{
    public class SignupVM
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

        [StringLength(maximumLength: 100, MinimumLength = 6, ErrorMessage = "Your Confirm Password must be between 6 and 100 characters long.")]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
