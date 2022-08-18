namespace YousifsProject.Views.Houses
{
    public class IndexVM
    {
        public int id { get; set; }
        public int NumberOfFloors { get; set; }
        public string Color { get; set; }
        public string TypeOfRoof { get; set; }
        public bool HaveWindow { get; set; }
        public bool HaveBalcony { get; set; }
        public bool HaveDoor { get; set; }
        public string Address { get; set; }

        public int HouseWidth { get; set; }


        public string GetWidth(double roofBorder = 1)
        {
            double width = (HouseWidth * 18 / 100 + 8)*roofBorder;
            return $"{width}rem";
        }
        public string GetWidthFloor()
        {
            int width = HouseWidth * 18 / 100 + 8 + 2;
            return width > 10 ? $"width: {width}rem" : $"width: {width}rem; font-size:12px";
        }
        public string GetStyle()
        {
            if (TypeOfRoof == "Flat Roof")
            {
                return $"border-bottom-color: {Color}; background-color: {Color}; border-left:0; border-right:0; width: {GetWidth()}";
            }
            else if (TypeOfRoof == "Triangle Roof")
            {
                return $"border-bottom-color: {Color};border-left: {GetWidth(0.5)} solid transparent; border-right: {GetWidth(0.5)} solid transparent;";
            }
            else if (TypeOfRoof == "Dome Roof")
            {
                return $"background-color: {Color}; border-radius:  {GetWidth(0.5)}  {GetWidth(0.5)} 0 0; border-bottom-color: {Color};  border-left: {GetWidth(0.5)} solid transparent; border-right: {GetWidth(0.5)} solid transparent;";
            }
            else
            {
                return "";
            }
        }
    }
}
