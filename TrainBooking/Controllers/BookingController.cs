using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;

namespace TrainBooking.Controllers
{
    public class BookingController : Controller
    {
        private IService<Station> _stationService;

        private readonly IMapper _mapper;
        public BookingController(IMapper mapper, IService<Station> stationService)
        {
            _mapper = mapper;
            _stationService = stationService;
        }
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Index");
            var stationList = await _stationService.GetAll();
            Console.WriteLine("stations: " + stationList);
            var booking = new BookingVM();
            booking.StationList = stationList != null ? stationList.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.Id.ToString(),
            }).ToList() : null;
            return View(booking);
        }
    }
}