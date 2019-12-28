using System.ComponentModel.DataAnnotations;

namespace FacePalm.Models
{
    public class Message
    {
        [Key]
        public string MessageId { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string Content { get; set; }
    }

}