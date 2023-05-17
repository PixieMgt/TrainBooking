using System.ComponentModel.DataAnnotations;

namespace TrainBooking.ViewModels
{
    public class PathVM
    {
        public PathVM()
        {
            SectionsVM = new();
        }
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public List<SectionVM?> SectionsVM { get; set; }

    }
}
