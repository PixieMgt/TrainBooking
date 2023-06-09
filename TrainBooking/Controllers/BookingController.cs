﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq;
using TrainBooking.Extensions;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.Util.Mail;
using TrainBooking.ViewModels;
using static System.Collections.Specialized.BitVector32;
using Section = TrainBooking.Models.Entities.Section;

namespace TrainBooking.Controllers
{
    public class BookingController : Controller
    {
        private IService<Station> _stationService;
        private IService<Section> _sectionService;
        private IService<Ticket> _ticketService;

        private readonly IMapper _mapper;
        public BookingController(IMapper mapper, IService<Station> stationService, IService<Section> sectionService, IService<Ticket> ticketService)
        {
            _mapper = mapper;
            _stationService = stationService;
            _sectionService = sectionService;
            _ticketService = ticketService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Station> stationList = null;
            try
            {
             stationList = await _stationService.GetAll();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var booking = new BookingVM();
            booking.StationList = stationList != null ? stationList.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.Id.ToString(),
            }).ToList() : null;
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BookingVM booking)
        {
            List<TicketVM> paths = new List<TicketVM>(); 
            if (!booking.departureStation.Equals(booking.arrivalStation))
            {
            var sectionList = await _sectionService.GetAll();
            var directConnections = sectionList.Where((a) => a.DepartureStation.Id.ToString().Equals(booking.departureStation)
                                                        && a.DestinationStation.Id.ToString().Equals(booking.arrivalStation)).ToList().OrderBy(s => s.DepartureTime);
            if (directConnections.Any()) 
            {
                foreach (var section in directConnections)
                {
                    var path = new TicketVM();
                    
                    path.SectionsVM.Add(_mapper.Map<SectionVM>(section));
                    paths.Add(path);
                }
            }else
            {
                var departures = sectionList.Where((a) => a.DepartureStation.Id.ToString().Equals(booking.departureStation)).OrderBy(s => s.DepartureTime).ToList();
                var middleSections = sectionList.Where((a) => isMiddleSection(a, booking)).OrderBy(s => s.DepartureTime).ToList();
                var destinations = sectionList.Where((a) => a.DestinationStation.Id.ToString().Equals(booking.arrivalStation)).OrderBy(s => s.DepartureTime).ToList();
                paths = makePaths(_mapper.Map<List<SectionVM>>(departures), _mapper.Map<List<SectionVM>>(middleSections), _mapper.Map<List<SectionVM>>(destinations));
            }
            for (var i = 0; i < paths.Count; i++)
            {
                paths[i].Id = i + 1;
                paths[i].Date = booking.departureDate;
            }
            HttpContext.Session.SetObject("PathList", paths);
            }

            var stationList = await _stationService.GetAll();
            booking.StationList = stationList != null ? stationList.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.Id.ToString(),
            }).ToList() : null;
            

            booking.Paths = paths;

            return View(booking);
        }
        
        public IActionResult Path(int id)
        {
            TicketVM? path;
            if(HttpContext.Session.GetObject<List<TicketVM>>("PathList") != null)
            {
                path = HttpContext.Session.GetObject<List<TicketVM>>("PathList").FirstOrDefault(s => s.Id == id);
            }
            else
            {
                path = new TicketVM();
            }
            return View(path);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Path(TicketVM pathVM)
        {
            TicketVM? currentPathVM;
            if (HttpContext.Session.GetObject<List<TicketVM>>("PathList") != null)
            {
                currentPathVM = HttpContext.Session.GetObject<List<TicketVM>>("PathList").FirstOrDefault(s => s.Id == pathVM.Id);
            }
            else
            {
                return NotFound();
            }
            if (currentPathVM != null)
            {
                currentPathVM.Date = pathVM.Date;
                currentPathVM.Class = pathVM.Class;
                CartItemVM item = new CartItemVM
                {
                    Id = currentPathVM.Id,
                    DepartureDate = currentPathVM.Date,
                    Sections = currentPathVM.SectionsVM,
                    SeatNumber = 1,
                    Class = currentPathVM.Class,
                    Price = 25,
                    Amount = 1
                };
                ShoppingCartVM? shopping;
                if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
                {
                    shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
                }
                else
                {
                    shopping = new ShoppingCartVM();
                    shopping.Cart = new List<CartItemVM>();
                }
                var lastCartItem = shopping.Cart.LastOrDefault();
                if (lastCartItem != null)
                {
                    item.Id = lastCartItem.Id + 1;
                }
                
                shopping?.Cart?.Add(item);
                HttpContext.Session.SetObject("ShoppingCart", shopping);
            }
            return RedirectToAction("Index", "ShoppingCart");
        }

        private bool isMiddleSection(Section section, BookingVM booking)
        {
            if (booking.departureStation.Equals("7") && new String[]{ "1", "3" }.Contains(booking.arrivalStation)) //Van Moskou naar London of Parijs
            { 
                if (section.DepartureStation.Id == 5 && section.DestinationStation.Id == 2)
                {
                    return true;
                }
            } else if (booking.arrivalStation.Equals("7") && new String[] { "1", "3" }.Contains(booking.departureStation)) //Van London of Parijs naar Moskou
            {
                if (section.DepartureStation.Id == 2 && section.DestinationStation.Id == 5)
                {
                    return true;
                }
            }
            return false;
        }

        private List<TicketVM> makePaths(List<SectionVM> departures, List<SectionVM> middleSections, List<SectionVM> destinations)
        {
            List<TicketVM> paths = new List<TicketVM>();
            if (middleSections.Count() > 0)
            {
                foreach (var departure in departures)
                {
                    
                    var middleSection = middleSections.FirstOrDefault(s => s.DepartureStation.Equals(departure.DestinationStation) && s.DepartureTime >= departure.ArrivalTime);
                    SectionVM? destination = null;
                    if (middleSection != null)
                    {
                        destination = destinations.FirstOrDefault(s => s.DepartureStation.Equals(middleSection.DestinationStation) && s.DepartureTime >= middleSection.ArrivalTime);
                    }
                    if (destination != null)
                    {
                        var path = new TicketVM();
                        path.SectionsVM.Add(departure);
                        path.SectionsVM.Add(middleSection);
                        path.SectionsVM.Add(destination);
                        paths.Add(path);
                    }
                }
            } else
            {
                foreach(var departure in departures)
                {
                    var destination = destinations.FirstOrDefault(s => s.DepartureStation.Equals(departure.DestinationStation) && s.DepartureTime >= departure.ArrivalTime);
                    if (destination != null)
                    {
                        var path = new TicketVM();
                        path.SectionsVM.Add(departure);
                        path.SectionsVM.Add(destination);
                        paths.Add(path);
                    }
                }
            }
            return paths;
        }

        /*public async Task<IActionResult> Ticket(int? Id, DateOnly departureDate, DateOnly arrivalDate, string selectedClass)
        {
            if (Id == null)
            {
                return NotFound();
            }

            Section? section = await _sectionService.FindById(Convert.ToInt32(Id));

            CartItemVM item = new CartItemVM
            {
                Id = Convert.ToInt32(Id),
                DepartureDate = departureDate,
                DepartureTime = section.DepartureTime,
                ArrivalDate = arrivalDate,
                ArrivalTime = section.ArrivalTime,
                DepartureStation = section.DepartureStation.City,
                DestinationStation = section.DestinationStation.City,
                Class = selectedClass
            };

            ShoppingCartVM? cart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            if (cart == null) 
            {
                cart = new ShoppingCartVM();
                cart.Cart = new List<CartItemVM>();
            }

            cart?.Cart?.Add(item);

            HttpContext.Session.SetObject("ShoppingCart", cart);

            return RedirectToAction("Index", "ShoppingCart");
        }*/
    }
}