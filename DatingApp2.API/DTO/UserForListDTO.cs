using System;

namespace DatingApp2.API.DTO
{
    public class UserForListDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }      
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoURL { get; set; }
    }
}