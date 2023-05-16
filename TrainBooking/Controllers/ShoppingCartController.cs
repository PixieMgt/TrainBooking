using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;
using TrainBooking.Extensions;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;

namespace TrainBooking.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private IService<Ticket> _ticketService;
        private IService<Booking> _bookingService;

        private readonly IMapper _mapper;

        public ShoppingCartController(IMapper mappper, IService<Ticket> ticketService, IService<Booking> bookingService)
        {
            _mapper = mappper;
            _ticketService = ticketService;
            _bookingService = bookingService;
        }
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
        public async Task<ActionResult> PaymentAsync(ShoppingCartVM? carts)
        {
            string? userID = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Booking? booking = new Booking()
            {
                UserId = userID
            };
            try
            {
                await _bookingService.Add(booking);
                var list = new List<Ticket>();
                foreach (var path in carts.Cart)
                {

                    Ticket ticket = new Ticket()
                    {
                        BookingId = booking.Id,
                        Price = path.Price,
                        SeatNumber = path.SeatNumber,
                        Date = DateTime.Parse(path.DepartureDate),
                        Sections = _mapper.Map<ICollection<Section>>(path.Sections)
                    };
                    list.Add(ticket);
                    
                }
                Console.WriteLine(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return View("index");
        }
    }
}
