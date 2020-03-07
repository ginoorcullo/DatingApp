using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp2.API.DTO
{
    public class UserForRegisterDTO
    {
        [Required(ErrorMessage="Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage="Password is required.")]
        [StringLength(8, MinimumLength=4,ErrorMessage="Password must be 4 to 8 characters long.")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }
        
        [Required]
        public string KnownAs { get; set; }
        
        [Required]        
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Country { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }

    }
}