using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class Chat
    {
      
        [Key]
        public int ChatId { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
        public virtual User User { get; set; }
        
        public Chat()
        {
            this.Conversations = new HashSet<Conversation>();
        }
    }
}