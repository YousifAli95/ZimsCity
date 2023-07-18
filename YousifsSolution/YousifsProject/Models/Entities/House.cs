﻿using YousifsProject.Models.Entities.IdentityClasses;

namespace YousifsProject.Models.Entities
{
    public partial class House
    {

        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int RoofId { get; set; }
        public bool HaveBalcony { get; set; }
        public bool HaveWindow { get; set; }
        public bool HaveDoor { get; set; }
        public int NumberOfFloors { get; set; }
        public int SortingOrder { get; set; }
        public string UserId { get; set; }
        public virtual AspNetUser UserNavigation { get; set; } = null!;
        public virtual Roof Roof { get; set; } = null!;

    }
}
