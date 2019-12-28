using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacePalm.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        [Required(ErrorMessage = "Albums need to have a title")]
        public string AlbumTitle { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}