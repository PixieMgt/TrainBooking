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
                
                foreach (var booking in userBookingList)
                {
                    foreach (var ticket in booking.Tickets)
                    {
                        var sectionList = await _sectionService.GetAll();
                        ticket.Sections = sectionList.Where(s => s.Tickets.Any(s => s.Id == ticket.Id)).ToList();
                        Console.WriteLine(ticket);
                    }
                }
                var bookingHistory = _mapper.Map<List<BookingHistoryVM>>(userBookingList);

                return View(bookingHistory);
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return NotFound();
        }
    }
}
