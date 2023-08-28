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
        public int Width { get; set; }


        public string GetRoofStyle()
        {
            string style = "";

            switch (TypeOfRoof)
            {
                case "Flat Roof":
                    style += $"border-bottom-color: {Color}; ";
                    style += $"background-color: {Color}; ";
                    style += $"border-left: {Width / 2}rem solid transparent; ";
                    style += $"border-right: {Width / 2}rem solid transparent; ";
                    break;

                case "Triangle Roof":
                    style += $"border-bottom-color: {Color}; ";
                    style += $"border-left: {Width / 2}rem solid transparent; ";
                    style += $"border-right: {Width / 2}rem solid transparent; ";
                    break;

                case "Dome Roof":
                    style += $"border-radius: {Width / 2}rem {Width / 2}rem 0 0; ";
                    style += $"border-bottom-color: {Color}; ";
                    style += $"background-color: {Color}; ";
                    style += $"border-left: {Width / 2}rem solid transparent; ";
                    style += $"border-right: {Width / 2}rem solid transparent; ";
                    break;

                default:
                    break;
            }

            return style;
        }
    }
}
