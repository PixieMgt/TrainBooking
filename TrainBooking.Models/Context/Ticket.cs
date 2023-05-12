using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Models.Context
{
    public class Ticket
    {
        public int Id { get; set; }
        public Booking Booking { get; set; }
        public int SeatNumber { get; set; }
        public double Price { get; set; }
        public ICollection<Section> Sections { get; set; }
       

    }
}
