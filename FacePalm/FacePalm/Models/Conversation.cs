using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }
        [Required]
        public int FirstUserId { get; set; }
        [Required]
        public int SecondUserId { get; set; }
        public List<int> MessagesIds { get; set; }
        public DateTime Date { get; set; }
    }
    public class ConversationDBContext : DbContext
    {
        public ConversationDBContext() : base("DBConnectionString") { }
        public DbSet<Conversation> Conversations { get; set; }
    }
}