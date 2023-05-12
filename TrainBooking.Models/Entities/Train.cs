using System;
using System.Collections.Generic;

namespace TrainBooking.Models.Entities
{
    public partial class Train
    {
        public int Id { get; set; }
        public int? EconomyClassCapacity { get; set; }
        public int? BusinessClassCapacity { get; set; }
    }
}
