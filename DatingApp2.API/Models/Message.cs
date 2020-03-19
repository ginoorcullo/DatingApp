using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp2.API.Models
{
    public class Message
    {

        public int Id { get; set; }
        public int SenderId { get; set; }

        [MaxLength(50)]
        public Users Sender { get; set; }
        public int RecipientId { get; set; }

        [MaxLength(50)]
        public Users Recipient { get; set; }        
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}