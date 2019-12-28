using System;
using System.ComponentModel.DataAnnotations;

namespace FacePalm.Models
{
    public class Comment
    {

        [Key]
        public int CommentId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public virtual Post Post { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }

}