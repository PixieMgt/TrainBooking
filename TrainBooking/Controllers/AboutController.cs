using Microsoft.AspNetCore.Mvc;

namespace TrainBooking.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
