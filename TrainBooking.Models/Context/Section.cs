using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Models.Context
{
    public class Section
    {
        public int Id { get; set; }
        public int? DepartureStationId { get; set; }
        public int? DestinationStationId { get; set; }
        public virtual Station DepartureStation { get; set; } = null!;
        public virtual Station DestinationStation { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public DateTime DestinationTime { get; set; }
        public int TrainId { get; set; }
        public Train Train { get; set; }
        public ICollection<SectionTicket> SectionTickets { get; set; }


    }
}
