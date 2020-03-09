using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp2.API.Data.BaseRepository;
using DatingApp2.API.DTO;
using DatingApp2.API.Helpers;
using DatingApp2.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp2.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var userEntity = await datingRepository.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if(String.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userEntity.Gender == "male" ? "female" : "male";
            }

            var result = await datingRepository.GetUsers(userParams);
            var resultToReturn = mapper.Map<IEnumerable<UserForListDTO>>(result);

            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(resultToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await datingRepository.GetUser(id);
            var resultToReturn = mapper.Map<UserDetailsDTO>(result);
            return Ok(resultToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForEditDTO user)
        {
            var result = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(id != int.Parse(result))
                return Unauthorized();
            
            var userEntity = await datingRepository.GetUser(id);
            mapper.Map(user, userEntity);

            if(await datingRepository.SaveAll())
                return NoContent();

            throw new Exception($"Updating user with id {id} failed on save.");
        }
    }
}