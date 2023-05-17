using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainBooking.Extensions;
using TrainBooking.Models.Entities;
using TrainBooking.Services;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;

namespace TrainBooking.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IService<Section> _sectionService;
        public PathController(IMapper mapper, IService<Section> sectionService)
        {
            _mapper = mapper;
            _sectionService = sectionService;
        }
        [HttpGet("{departureStationId}/{destinationStationId}")]
        public async Task<IEnumerable<PathVM>> Get(int departureStationId, int destinationStationId)
        {
            List<PathVM> paths = new List<PathVM>();
            var sectionList = await _sectionService.GetAll();
            var directConnections = sectionList.Where((a) => a.DepartureStation.Id == departureStationId
                                                        && a.DestinationStation.Id == destinationStationId).ToList().OrderBy(s => s.DepartureTime);
            if (directConnections.Count() > 0)
            {
                foreach (var section in directConnections)
                {
                    var path = new PathVM();

                    path.SectionsVM.Add(_mapper.Map<SectionVM>(section));
                    paths.Add(path);
                }
            }
            else
            {
                var departures = sectionList.Where((a) => a.DepartureStation.Id == departureStationId).OrderBy(s => s.DepartureTime).ToList();
                var middleSections = sectionList.Where((a) => isMiddleSection(a, departureStationId, destinationStationId)).OrderBy(s => s.DepartureTime).ToList();
                var destinations = sectionList.Where((a) => a.DestinationStation.Id == destinationStationId).OrderBy(s => s.DepartureTime).ToList();
                paths = makePaths(_mapper.Map<List<SectionVM>>(departures), _mapper.Map<List<SectionVM>>(middleSections), _mapper.Map<List<SectionVM>>(destinations));
            }

            for (var i = 0; i < paths.Count; i++)
            {
                paths[i].Id = i;
            }
            
            return paths;
        }

        private bool isMiddleSection(Section section, int departureStationId, int destinationStationId)
        {
            if (departureStationId == 7 && new int[] { 1, 3 }.Contains(destinationStationId)) //Van Moskou naar London of Parijs
            {
                if (section.DepartureStation.Id == 5 && section.DestinationStation.Id == 2)
                {
                    return true;
                }
            }
            else if (destinationStationId == 7 && new int[] { 1, 3 }.Contains(departureStationId)) //Van London of Parijs naar Moskou
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
            }
            else
            {
                foreach (var departure in departures)
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

    }
}
