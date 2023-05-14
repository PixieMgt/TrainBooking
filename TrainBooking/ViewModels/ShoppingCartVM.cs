namespace TrainBooking.ViewModels
{
    public class ShoppingCartVM
    {
        public List<CartItemVM>? Cart { get; set; }
    }

    public class CartItemVM
    {
        public int Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
    }
}
