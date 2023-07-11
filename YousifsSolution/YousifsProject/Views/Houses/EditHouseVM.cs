using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace YousifsProject.Views.Houses
{
    public class EditHouseVM
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "&#8592; Write an address")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Number of Floors")]
        public int NumberOfFloors { get; set; }

        [Display(Name = "Color of the House")]
        public string Color { get; set; }

        public string TypeOfRoof { get; set; }
        [Display(Name = "Windows")]
        public bool HaveWindow { get; set; }
        [Display(Name = "Balcony")]
        public bool HaveBalcony { get; set; }
        [Display(Name = "Door")]
        public bool HaveDoor { get; set; }

        public string[]? TypeOfRoofsArray { get; set; }

        public string GetStyle()
        {
            if (TypeOfRoof == "Flat Roof")
            {
                return $"border-bottom-color: {Color}; background-color: {Color};";
            }
            else if (TypeOfRoof == "Triangle Roof")
            {
                return $"border-bottom-color: {Color}";
            }
            else if (TypeOfRoof == "Dome Roof")
            {
                return $"background-color: {Color}; border-radius: 6rem 6rem 0 0; border-bottom-color: {Color} ";
            }
            else
            {
                return "";
            }
        }

        public SelectListItem[]? FloorArray { get; set; }



    }
}
