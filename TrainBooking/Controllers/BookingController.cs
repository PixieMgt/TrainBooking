using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq;
using TrainBooking.Extensions;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;
using static System.Collections.Specialized.BitVector32;
using Section = TrainBooking.Models.Entities.Section;

namespace TrainBooking.Controllers
{
    public class BookingController : Controller
    {
        private IService<Station> _stationService;
        private IService<Section> _sectionService;

        private readonly IMapper _mapper;
        public BookingController(IMapper mapper, IService<Station> stationService, IService<Section> sectionService)
        {
            _mapper = mapper;
            _stationService = stationService;
            _sectionService = sectionService;
        }
        public async Task<IActionResult> Index()
        {
            var stationList = await _stationService.GetAll();
            var booking = new BookingVM();
            booking.StationList = stationList != null ? stationList.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.Id.ToString(),
            }).ToList() : null;
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookingVM booking)
        {
            List<PathVM> paths = new List<PathVM>(); 
            var sectionList = await _sectionService.GetAll();
            var directConnections = sectionList.Where((a) => a.DepartureStation.Id.ToString().Equals(booking.departureStation)
                                                        && a.DestinationStation.Id.ToString().Equals(booking.arrivalStation)).ToList().OrderBy(s => s.DepartureTime);
            if (directConnections.Count() > 0) 
            {
                foreach (var section in directConnections)
                {
                    var path = new PathVM();
                    
                    path.SectionsVM.Add(_mapper.Map<SectionVM>(section));
                    paths.Add(path);
                }
            }else
            {
                var departures = sectionList.Where((a) => a.DepartureStation.Id.ToString().Equals(booking.departureStation)).OrderBy(s => s.DepartureTime).ToList();
                List<Section> middleSections = sectionList.Where((a) => isMiddleSection(a, booking)).OrderBy(s => s.DepartureTime).ToList();
                var destinations = sectionList.Where((a) => a.DestinationStation.Id.ToString().Equals(booking.arrivalStation)).OrderBy(s => s.DepartureTime).ToList();
                paths = makePaths(_mapper.Map<List<SectionVM>>(departures), _mapper.Map<List<SectionVM>>(middleSections), _mapper.Map<List<SectionVM>>(destinations));
            }

            var stationList = await _stationService.GetAll();
            booking.StationList = stationList != null ? stationList.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.Id.ToString(),
            }).ToList() : null;
            for (var i = 0; i < paths.Count; i++)
            {
                paths[i].Id = i;
            }
            HttpContext.Session.SetObject("PathList", paths);
            booking.Paths = paths;

            return View(booking);
        }
        
        public IActionResult Path(int id)
        {
            PathVM? path;
            if(HttpContext.Session.GetObject<List<PathVM>>("PathList") != null)
            {
                path = HttpContext.Session.GetObject<List<PathVM>>("PathList").First(s => s.Id == id);
            }
            else
            {
                path = new PathVM();
            }
            return View(path);
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

        private List<PathVM> makePaths(List<SectionVM> departures, List<SectionVM> middleSections, List<SectionVM> destinations)
        {
            List<PathVM> paths = new List<PathVM>();
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
                        var path = new PathVM();
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
                        var path = new PathVM();
                        path.SectionsVM.Add(departure);
                        path.SectionsVM.Add(destination);
                        paths.Add(path);
                    }
                }
            }
            return paths;
        }

        public async Task<IActionResult> Ticket(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            Section? section = await _sectionService.FindById(Convert.ToInt32(Id));

            CartItemVM item = new CartItemVM
            {
                Id = Convert.ToInt32(Id),
                DepartureTime = section.DepartureTime,
                ArrivalTime = section.ArrivalTime,
                DepartureStation = section.DepartureStation.City,
                DestinationStation = section.DestinationStation.City
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
        }
    }
}