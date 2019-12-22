using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class Comment
    {

        [Key]
        public string CommentId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
    
}