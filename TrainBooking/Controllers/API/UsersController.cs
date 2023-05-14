using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.Models.Entities;
using TrainBooking.Services.Interfaces;
using TrainBooking.ViewModels;

namespace TrainBooking.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IService<AspNetUser> _userService;
        public UsersController(IMapper mapper, IService<AspNetUser> userService) {
            _mapper = mapper;
            _userService = userService;
        }
        // GET: api/<StationsController>
        [HttpGet]
        public async Task<IEnumerable<UserVM>> Get()
        {
            var users = await _userService.GetAll();
            return _mapper.Map<List<UserVM>>(users);
        }

        // GET api/<StationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
