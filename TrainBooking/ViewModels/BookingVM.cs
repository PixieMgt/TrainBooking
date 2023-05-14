using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TrainBooking.Models.Entities;

namespace TrainBooking.ViewModels
{
    public class BookingVM
    {
        [Display(Name = "From")]
        [Required(ErrorMessage = "Please select a departure station")]
        public string departureStation { get; set; }

        [Display(Name = "To")]
        [Required(ErrorMessage = "Please select an arrival station")]
        public string arrivalStation { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please enter a departure date")]
        public DateOnly departureDate { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "Please enter a departure time")]
        public TimeSpan departureTime { get; set; }

        public List<SelectListItem>? StationList { get; set; }

        public IEnumerable<PathVM> Paths { get; set; }
    }
}
