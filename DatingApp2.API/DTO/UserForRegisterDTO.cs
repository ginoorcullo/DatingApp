using System.ComponentModel.DataAnnotations;

namespace DatingApp2.API.DTO
{
    public class UserForRegisterDTO
    {
        [Required(ErrorMessage="Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage="Password is required.")]
        [StringLength(12, MinimumLength=8,ErrorMessage="Password must be 8 to 12 characters long.")]
        public string Password { get; set; }
    }
}