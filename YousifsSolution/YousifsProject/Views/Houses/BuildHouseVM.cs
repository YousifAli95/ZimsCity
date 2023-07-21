﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace YousifsProject.Views.Houses
{
    public class BuildHouseVM
    {
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "The address must be at least 4 letters")]
        [Required(ErrorMessage = "Write an address")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Number of Floors")]
        public int NumberOfFloors { get; set; }

        [Required]
        [Display(Name = "Color of the House")]
        public string Color { get; set; }

        [Required]
        [Display(Name = "Windows")]
        public bool HaveWindow { get; set; }

        [Required]
        [Display(Name = "Balcony")]
        public bool HaveBalcony { get; set; }

        [Required]
        [Display(Name = "Door")]
        public bool HaveDoor { get; set; }

        [Required]
        public string TypeOfRoof { get; set; }

        public SelectListItem[]? FloorArray { get; set; }
    }
}
