namespace TrainBooking.ViewModels
{
    public class ShoppingCartVM
    {
        public List<CartItemVM>? Cart { get; set; }
    }

    public class CartItemVM
    {
        public int Id { get; set; }
        public string DepartureDate { get; set; }
        public List<SectionVM> Sections { get; set; }
        public int SeatNumber { get; set; }
        public string Class { get; set; }
    }
}
