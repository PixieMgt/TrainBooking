using TrainBooking.Models.Entities;

namespace TrainBooking.ViewModels
{
    public class PathVM
    {
        public PathVM()
        {
            Sections = new List<Section>();
        }
        public List<Section?> Sections { get; set; }
        public Station DepartureStation { get; set; }
        public Station DestinationStation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime DestinationTime { get; set; }
    }
}
