namespace TrainBooking.ViewModels
{
    public class SectionVM
    {
        public DateTime DepartureTime { get; set; }
        public DateTime DestinationTime { get; set; }
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        public int EconomyClassCapacity { get; set; }
        public int BusinessClassCapacity { get; set; }

    }
}
