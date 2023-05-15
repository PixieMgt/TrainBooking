using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrainBooking.Extensions;
using TrainBooking.ViewModels;

namespace TrainBooking.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            return View(cartList);
        }

        public IActionResult Delete(int? Id)
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            cartList.Cart.Remove(cartList.Cart.FirstOrDefault(r => r.Id == Id));
            HttpContext.Session.SetObject("ShoppingCart", cartList);
            return View("Index", cartList);
        }

        [HttpPost] 
        public ActionResult Payment(ShoppingCartVM? cart)
        {
            string? userID = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return null;
        }
    }
}
