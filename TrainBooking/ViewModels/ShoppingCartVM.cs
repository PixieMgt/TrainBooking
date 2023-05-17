using System.ComponentModel.DataAnnotations;

namespace TrainBooking.ViewModels
{
    public class ShoppingCartVM
    {
        public List<CartItemVM>? Cart { get; set; }
    }

    public class CartItemVM
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public string DepartureDate { get; set; }
        public List<SectionVM>? Sections { get; set; }
        public int SeatNumber { get; set; }
        public int Price { get; set; }
        public string Class { get; set; }
        public int Amount { get; set; }
    }
}
