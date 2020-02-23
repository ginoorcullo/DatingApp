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
    }
}