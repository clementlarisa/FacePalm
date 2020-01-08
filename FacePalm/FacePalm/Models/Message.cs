using System.ComponentModel.DataAnnotations;

namespace FacePalm.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Content { get; set; }
        public int ConverstionId { get; set; }
        public virtual Conversation Conversation { get; set; }
        public System.DateTime Date { get; set; }
    }

}