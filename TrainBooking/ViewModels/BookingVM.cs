﻿using System.ComponentModel.DataAnnotations;

namespace TrainBooking.ViewModels
{
    public class BookingVM
    {
        [Display(Name = "From")]
        [Required(ErrorMessage = "Please enter a departure station")]
        public string departureStation { get; set; }

        [Display(Name = "To")]
        [Required(ErrorMessage = "Please enter an arrival station")]
        public string arrivalStation { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please enter a departure date")]
        public DateOnly departureDate { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "Please enter a departure time")]
        public TimeOnly departureTime { get; set; }
    }
}
