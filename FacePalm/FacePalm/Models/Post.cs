using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace FacePalm.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        [DisplayName("Content")]
        public string ReadableContent { get; set; }
        [DisplayName("Upload Photo")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set;  }
        public DateTime Date { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}