using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class Album
    {
        [Key]
        public string AlbumId { get; set; }
        [Required]
        //public List<Bitmap> Images { get; set; } 
        public List<string> CommentsIds { get; set; }
        public List<string> PostsIds { get; set; }
    }
}