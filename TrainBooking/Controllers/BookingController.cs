using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;

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
                    path.Sections.Add(section);
                    path.DepartureTime = section.DepartureTime;
                    path.DepartureStation = section.DepartureStation;
                    path.DestinationTime = section.DestinationTime;
                    path.DestinationStation = section.DestinationStation;
                    paths.Add(path);
                }
            }else
            {
                var departures = sectionList.Where((a) => a.DepartureStation.Id.ToString().Equals(booking.departureStation)).OrderBy(s => s.DepartureTime).ToList();
                List<Section> inbetweens = sectionList.Where((a) => isBetweenLocations(a, booking)).OrderBy(s => s.DepartureTime).ToList();
                var destinations = sectionList.Where((a) => a.DestinationStation.Id.ToString().Equals(booking.arrivalStation)).OrderBy(s => s.DepartureTime).ToList();
                paths = makePaths(departures, inbetweens, destinations);
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

        private bool isBetweenLocations(Section section, BookingVM booking)
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
        private List<PathVM> makePaths(List<Section> departures, List<Section> inbetweens, List<Section> destinations)
        {
            List<PathVM> paths = new List<PathVM>();
            if (inbetweens.Count() > 0)
            {
                foreach (var departure in departures)
                {
                    
                    var inbetween = inbetweens.FirstOrDefault(s => s.DepartureStation.Equals(departure.DestinationStation) && s.DepartureTime >= departure.DestinationTime);
                    Section? destination = null;
                    if (inbetween != null)
                    {
                        destination = destinations.FirstOrDefault(s => s.DepartureStation.Equals(inbetween.DestinationStation) && s.DepartureTime >= inbetween.DestinationTime);
                    }
                    if (destination != null)
                    {
                        var path = new PathVM();
                        path.DepartureStation = departure.DepartureStation;
                        path.DepartureTime = departure.DepartureTime;
                        path.DestinationStation = destination.DestinationStation;
                        path.DestinationTime = destination.DestinationTime;
                        path.Sections.Add(departure);
                        path.Sections.Add(inbetween);
                        path.Sections.Add(destination);
                        paths.Add(path);
                    }
                }
            } else
            {
                foreach(var departure in departures)
                {
                    var destination = destinations.FirstOrDefault(s => s.DepartureStation.Equals(departure.DestinationStation) && s.DepartureTime >= departure.DestinationTime);
                    if (destination != null)
                    {
                        var path = new PathVM();
                        path.DepartureStation = departure.DepartureStation;
                        path.DepartureTime = departure.DepartureTime;
                        path.DestinationStation = destination.DestinationStation;
                        path.DestinationTime = destination.DestinationTime;
                        path.Sections.Add(departure);
                        path.Sections.Add(destination);
                        paths.Add(path);
                    }
                }
            }
            return paths;
        }
    }
}