using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Models.Context
{
    public class SectionTicket
    {
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
