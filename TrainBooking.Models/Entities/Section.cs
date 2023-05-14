using System;
using System.Collections.Generic;

namespace TrainBooking.Models.Entities
{
    public partial class Section
    {
        public Section()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan DestinationTime { get; set; }
        public int TrainId { get; set; }

        public virtual Station DepartureStation { get; set; } = null!;
        public virtual Station DestinationStation { get; set; } = null!;
        public virtual Train Train { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
