using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Models
{
    public class Post
    {
        public Post()
        {
            Album = null;
            AlbumId = -1;
        }
        [Key]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [DisplayName("Description")]
        public string ImageDescription { get; set; }
        [DisplayName("Upload Photo")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public int? AlbumId { get; set; }
        public virtual Album Album { get; set; }
        public IEnumerable<SelectListItem> Albums { get; set; }
    }
}