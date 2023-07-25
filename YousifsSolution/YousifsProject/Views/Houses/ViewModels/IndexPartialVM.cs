namespace YousifsProject.Views.Houses.ViewModels
{
    public class IndexPartialVM
    {
        public int id { get; set; }
        public int NumberOfFloors { get; set; }
        public string Color { get; set; }
        public string TypeOfRoof { get; set; }
        public bool HaveWindow { get; set; }
        public bool HaveBalcony { get; set; }
        public bool HaveDoor { get; set; }
        public string Address { get; set; }
        public int SortingOrder { get; set; }



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
    }
}
