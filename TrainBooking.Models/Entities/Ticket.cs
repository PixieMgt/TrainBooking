using System;
using System.Collections.Generic;

namespace TrainBooking.Models.Entities
{
    public partial class Ticket
    {
        public Ticket()
        {
            Sections = new HashSet<Section>();
        }

        public int Id { get; set; }
        public int BookingId { get; set; }
        public int? SeatNumber { get; set; }
        public double? Price { get; set; }
        public DateTime Date { get; set; }
        public string? Class { get; set; }

        public virtual Booking Booking { get; set; } = null!;

        public virtual ICollection<Section> Sections { get; set; }
    }
}
