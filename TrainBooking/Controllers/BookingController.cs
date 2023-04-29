using Microsoft.AspNetCore.Mvc;

namespace TrainBooking.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}