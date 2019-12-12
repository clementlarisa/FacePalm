using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int ReceiverId { get; set; }
        [Required]
        public string Content { get; set; }
    }

    public class MessageDBContext : DbContext
    {
        public MessageDBContext() : base("DBConnectionString") { }
        public DbSet<Message> Messages { get; set; }
    }
}