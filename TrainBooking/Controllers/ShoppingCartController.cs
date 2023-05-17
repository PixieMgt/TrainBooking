using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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

        private readonly IEmailSender _emailSender;

        public ShoppingCartController(IMapper mappper, IService<Ticket> ticketService, IService<Booking> bookingService, IEmailSender emailSender)

        {
            _mapper = mappper;
            _ticketService = ticketService;
            _bookingService = bookingService;
            _emailSender = emailSender;
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
                        await _ticketService.Add(ticket);
                        totalPrice += cartItem.Price;
                        message += "Date: " + DateTime.Parse(cartItem.DepartureDate).ToShortDateString() + Environment.NewLine;
                        message += "From: " + ticket.Sections.FirstOrDefault().DestinationStation + Environment.NewLine;
                        message += "To: " + ticket.Sections.LastOrDefault().DestinationStation + Environment.NewLine;
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
            _emailSender.SendEmailAsync(email, "Booking Confirmation", message);

            HttpContext.Session.SetObject("ShoppingCart", null);

            return Redirect("confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
