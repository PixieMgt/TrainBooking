using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainBooking.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IService<Station> _stationService;
        public StationsController(IMapper mapper, IService<Station> stationService)
        {
            _mapper = mapper;
            _stationService = stationService;
        }
        // GET: api/<StationsController>
        [HttpGet]
        public async Task<IEnumerable<StationVM>> Get()
        {
            var stations = await _stationService.GetAll();
            return _mapper.Map<List<StationVM>>(stations);
        }

        // GET api/<StationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
