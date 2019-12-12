using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Content { get; set; }
        //For further infos: https://docs.microsoft.com/en-us/dotnet/api/system.drawing.bitmap?view=netframework-4.8
        public Bitmap Image { get; set; } 
        public DateTime Date { get; set; }
    }
    public class PostDBContext : DbContext
    {
        public PostDBContext() : base("DBConnectionString") { }
        public DbSet<Post> Posts { get; set; }
    }
}