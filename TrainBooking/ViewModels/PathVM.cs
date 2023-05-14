using TrainBooking.Models.Entities;

namespace TrainBooking.ViewModels
{
    public class PathVM
    {
        public PathVM()
        {
            SectionsVM = new List<SectionVM>();
        }
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public List<SectionVM?> SectionsVM { get; set; }
    }
}
