using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp2.API.Data.BaseRepository;
using DatingApp2.API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController: ControllerBase
    {
        private readonly IDatingRepository datingRepository;
        private readonly IMapper mapper;

        public UsersController(IDatingRepository datingRepository, IMapper mapper)
        {
            this.datingRepository = datingRepository;
            this.mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await datingRepository.GetUsers();
            var resultToReturn = mapper.Map<IEnumerable<UserForListDTO>>(result);
            return Ok(resultToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await datingRepository.GetUser(id);
            var resultToReturn = mapper.Map<UserDetailsDTO>(result);
            return Ok(resultToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            return Ok();
        }
    }
}