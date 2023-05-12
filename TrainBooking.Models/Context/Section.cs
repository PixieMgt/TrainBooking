using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Models.Context
{
    public class Section
    {
        public int Id { get; set; }
        public Station DepartureStation { get; set; }
        public Station DestinationStation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime DestinationTime { get; set; }
        public Train Train { get; set; }
    }
}
