using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;

namespace TrainBooking.Controllers
{
    [Authorize]
    public class BookingHistoryController : Controller
    {
        private IService<Booking> _bookingService;
        private IService<Ticket> _ticketService;
        private IService<Section> _sectionService;

        private IMapper _mapper;

        public BookingHistoryController (IMapper mapper, IService<Booking> bookingService, IService<Ticket> ticketService, IService<Section> sectionService)
        {
            _mapper = mapper;
            _bookingService = bookingService;
            _ticketService = ticketService;
            _sectionService = sectionService;
        }
        public async Task<IActionResult> Index()
        {
            string? userID = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var bookingList = await _bookingService.GetAll();
                var userBookingList = bookingList.Where(s => s.UserId.Equals(userID)).ToList();
                List<double?> prices = new();
                foreach (var booking in userBookingList)
                {
                    double? totalPrice = 0;
                    foreach (var ticket in booking.Tickets)
                    {
                        totalPrice += ticket.Price;
                        var sectionList = await _sectionService.GetAll();
                        ticket.Sections = sectionList.Where(s => s.Tickets.Any(s => s.Id == ticket.Id)).ToList();
                    }
                    prices.Add(totalPrice);
                }
                var bookingHistory = _mapper.Map<List<BookingHistoryVM>>(userBookingList);
                for (int i = 0; i < bookingHistory.Count; i++)
                {
                    bookingHistory[i].TotalPrice = prices[i];
                }
                return View(bookingHistory.OrderByDescending(s => s.CreationDate));
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return NotFound();
        }

        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var ticket = await _ticketService.FindById((int)id);
                await _ticketService.Delete(ticket);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return NotFound();

        }
    }
}
