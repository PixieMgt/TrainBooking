using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Models.Context
{
    public class Train
    {
        public int Id { get; set; }
        public int EconomyClassCapacity { get; set; }
        public int BusinessClassCapacity { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
