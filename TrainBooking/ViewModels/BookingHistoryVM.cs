using TrainBooking.Models.Entities;

namespace TrainBooking.ViewModels
{
    public class BookingHistoryVM
    {
        public List<TicketVM> TicketList { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
