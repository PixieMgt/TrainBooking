using System.ComponentModel.DataAnnotations;
using TrainBooking.Models.Entities;

namespace TrainBooking.ViewModels
{
    public class TicketVM
    {
        public TicketVM()
        {
            SectionsVM = new List<SectionVM>();
        }
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }
        public List<SectionVM?> SectionsVM { get; set; }
        public string Class { get; set; }
    }
}
