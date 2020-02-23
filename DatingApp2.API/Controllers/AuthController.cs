using System.Threading.Tasks;
using DatingApp2.API.Data.BaseRepository;
using DatingApp2.API.Models;
using Microsoft.AspNetCore.Mvc;
using DatingApp2.API.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        }

        [HttpPost("register")]        
        public async Task<IActionResult> RegisterUser(UserForRegisterDTO user)
        {
            /* Validate user */
            user.Username = user.Username.ToLower();
            
            if(await repo.UserExist(user.Username))
                return BadRequest($"User {user.Username} is already existing.");
            
            var userToCreate = new Users
            {
                Username = user.Username
            };

            var createdUser = await repo.Register(userToCreate, user.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForRegisterDTO user)
        {
            var userFromRepo = await repo.Login(user.Username.ToLower(), user.Password);

            if(userFromRepo == null)
                return Unauthorized();
            
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}