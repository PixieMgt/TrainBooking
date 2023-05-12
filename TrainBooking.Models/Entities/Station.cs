using System;
using System.Collections.Generic;

namespace TrainBooking.Models.Entities
{
    public partial class Station
    {
        public Station()
        {
            SectionDepartureStations = new HashSet<Section>();
            SectionDestinationStations = new HashSet<Section>();
        }

        public int Id { get; set; }
        public string? City { get; set; }

        public virtual ICollection<Section> SectionDepartureStations { get; set; }
        public virtual ICollection<Section> SectionDestinationStations { get; set; }
    }
}
