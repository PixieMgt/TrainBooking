using System;
using System.Collections.Generic;

namespace TrainBooking.Entities
{
    public partial class Train
    {
        public Train()
        {
            Sections = new HashSet<Section>();
        }

        public int Id { get; set; }
        public int? EconomyClassCapacity { get; set; }
        public int? BusinessClassCapacity { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}
