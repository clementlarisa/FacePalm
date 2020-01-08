using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using static FacePalm.Enums.FacePalmEnums;

namespace FacePalm.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Education { get; set; }
        [Required]
        public GenderTypes Gender { get; set; }
        public string Job { get; set; }
        [Required]
        public RelationshipStatus RelationshipStatus { get; set; }
        //public Bitmap ProfilePicture { get; set; }
        [Required]
        public ProfilePrivacyTypes ProfilePrivacy { get; set; }
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public virtual ICollection<User> Friends { get; set; }
        public List<string> AlbumsIds { get; set; }
        public List<string> ConversationsIds { get; set; }
        public List<string> GroupsIds { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }

}