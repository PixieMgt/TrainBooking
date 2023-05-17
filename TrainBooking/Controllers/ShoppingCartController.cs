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
        private IService<Section> _sectionService;

        private readonly IMapper _mapper;

        public ShoppingCartController(IMapper mappper, IService<Ticket> ticketService, IService<Booking> bookingService, IService<Section> sectionService)
        {
            _mapper = mappper;
            _ticketService = ticketService;
            _bookingService = bookingService;
            _sectionService = sectionService;
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
                UserId = userID,
                CreationDate = DateTime.Now
            };
            try
            {
                await _bookingService.Add(booking);
                foreach (var cartItem in carts.Cart)
                {
                    
                    for (var i = 0; i < cartItem.Amount; i++)
                    {
                        Ticket ticket = new Ticket()
                        {
                            BookingId = booking.Id,
                            Price = cartItem.Price,
                            SeatNumber = cartItem.SeatNumber,
                            Date = DateTime.Parse(cartItem.DepartureDate),
                            Sections = _mapper.Map<ICollection<Section>>(cartItem.Sections)
                        };
                        await _ticketService.Add(ticket);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return View("confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
