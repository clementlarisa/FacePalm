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
        public string PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        public string ReadableContent { get; set; }
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set;  }
        public DateTime Date { get; set; }
    }
}