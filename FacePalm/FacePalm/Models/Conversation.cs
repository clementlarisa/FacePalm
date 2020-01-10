using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacePalm.Models
{

  
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }
        public string Name { get; set; }
        [Required]
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        [Required]
        public virtual ICollection<Chat> Chats {get;set;}

        public Conversation()
        {
            this.Users = new HashSet<User>();
            this.Messages = new HashSet<Message>();
            this.Chats = new HashSet<Chat>();

        }
    }

}