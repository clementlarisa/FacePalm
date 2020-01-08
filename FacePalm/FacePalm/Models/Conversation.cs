using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacePalm.Models
{
    public class Conversation
    {
        public Conversation()
        {
            this.Users = new HashSet<User>();
        }
        [Key]
        public string ConversationId { get; set; }
        [Required]
        public virtual ICollection<User> Users { get; set; }
        [Required]
        public string SecondUserId { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public DateTime Date { get; set; }
    }

}