using System;
using System.Collections.Generic;

namespace YousifsProject.Models.Entities
{
    public partial class Roof
    {
        public Roof()
        {
            Houses = new HashSet<House>();
        }

        public int Id { get; set; }
        public string TypeOfRoof { get; set; } = null!;

        public virtual ICollection<House> Houses { get; set; }
    }
}
