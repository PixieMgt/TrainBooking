using NuGet.Packaging.Signing;

namespace TrainBooking.ViewModels
{
    public class SectionVM
    {
        public int Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        //public int EconomyClassCapacity { get; set; }
        //public int BusinessClassCapacity { get; set; }
        public TrainVM Train { get; set; }

    }
}
