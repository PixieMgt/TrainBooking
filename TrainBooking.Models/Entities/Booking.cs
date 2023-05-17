using System;
using System.Collections.Generic;

namespace TrainBooking.Models.Entities
{
    public partial class Booking
    {
        public Booking()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual AspNetUser? User { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
