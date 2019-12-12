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
        public int AlbumId { get; set; }
        [Required]
        public List<Bitmap> Images { get; set; } 
        public List<int> CommentsIds { get; set; }
        public List<int> PostsIds { get; set; }
    }
    public class AlbumDBContext : DbContext
    {
        public AlbumDBContext() : base("DBConnectionString") { }
        public DbSet<Album> Albums { get; set; }
    }
}