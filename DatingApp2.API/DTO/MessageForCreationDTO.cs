using System;

namespace DatingApp2.API.DTO
{
    public class MessageForCreationDTO
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        public MessageForCreationDTO()
        {
            this.MessageSent = DateTime.Now;
        }
    }
}