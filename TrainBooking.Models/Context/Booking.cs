using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Models.Context
{
    public class Booking
    {
        public int Id { get; set; }
        //public Account Account { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
