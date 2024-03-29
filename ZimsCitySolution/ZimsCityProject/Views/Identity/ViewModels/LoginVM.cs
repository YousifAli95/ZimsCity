﻿using System.ComponentModel.DataAnnotations;

namespace ZimsCityProject.Views.Identity.ViewModels
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

