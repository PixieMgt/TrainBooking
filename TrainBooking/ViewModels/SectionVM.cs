namespace TrainBooking.ViewModels
{
    public class SectionVM
    {
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        public int EconomyClassCapacity { get; set; }
        public int BusinessClassCapacity { get; set; }

    }
}
