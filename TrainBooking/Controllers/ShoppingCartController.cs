using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrainBooking.Extensions;
using TrainBooking.Models.Entities;
using TrainBooking.Services;
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
        private IService<Station> _stationService;

        private readonly IMapper _mapper;

        private readonly IEmailSender _emailSender1;
        private readonly IEmailSender2 _emailSender2;

        public ShoppingCartController(IMapper mappper, IService<Ticket> ticketService, IService<Booking> bookingService, IService<Section> sectionService, IEmailSender2 emailSender2, IService<Station> stationService, IEmailSender emailSender1)

        {
            _mapper = mappper;
            _ticketService = ticketService;
            _bookingService = bookingService;
            _sectionService = sectionService;
            _emailSender1 = emailSender1;
            _emailSender2 = emailSender2;
            _stationService = stationService;
        }
        public IActionResult Index()
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            return View(cartList);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            try
            {
                cartList.Cart.Remove(cartList.Cart.FirstOrDefault(r => r.Id == Id));
                HttpContext.Session.SetObject("ShoppingCart", cartList);
                return View("Index", cartList);
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PaymentAsync(ShoppingCartVM? carts)
        {
            int totalPrice = 0;
            string message = "";

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
                            Class = cartItem.Class,
                            Sections = _mapper.Map<ICollection<Section>>(cartItem.Sections)
                        };
                        var from = await _stationService.FindById(ticket.Sections.FirstOrDefault().DepartureStationId);
                        var to = await _stationService.FindById(ticket.Sections.LastOrDefault().DestinationStationId);
                        await _ticketService.Add(ticket);
                        totalPrice += cartItem.Price;
                        message += "Date: " + DateTime.Parse(cartItem.DepartureDate).ToShortDateString() + Environment.NewLine;
                        message += "From: " + from.City + Environment.NewLine;
                        message += "To: " + to.City + Environment.NewLine;
                        message += "Departure Time: " + ticket.Sections.FirstOrDefault().DepartureTime + Environment.NewLine;
                        message += "Arrival Time: " + ticket.Sections.LastOrDefault().ArrivalTime + Environment.NewLine;
                        message += "Class: " + cartItem.Class + Environment.NewLine;
                        message += "Amount: " + cartItem.Amount + Environment.NewLine;
                        message += "Subtotal: " + cartItem.Price * cartItem.Amount + Environment.NewLine;
                        message += Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }



            string? email = User?.FindFirst(ClaimTypes.Email)?.Value;

            message += "Total: $" + totalPrice;
            try
            {
                _emailSender2.SendEmailAsync(email, "Booking Confirmation", message); //without pdf
                _emailSender1.SendEmailAsync(email, "Booking Confirmation", message); //with pdf...
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            HttpContext.Session.SetObject("ShoppingCart", null);

            return Redirect("confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
