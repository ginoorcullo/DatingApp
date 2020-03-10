using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp2.API.Models
{
    public class Users
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Column(TypeName="varchar(50)")]        
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [MaxLength(7)]
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        [Column(TypeName="varchar(50)")]        
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }

        [MaxLength(50)]        
        [Column(TypeName="varchar(50)")]        
        public string City { get; set; }

        [MaxLength(50)]        
        [Column(TypeName="varchar(50)")]        
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> Likers {get; set;}
        public ICollection<Like> Likees {get; set;}
    }
}