using FacePalm.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using static FacePalm.Enums.ProfilePrivacyEnum;

namespace FacePalm.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Bitmap ProfilePicture { get; set; }
        public ProfilePrivacyEnum ProfilePrivacy { get; set; }
        public Dictionary<int,int> FriendsIds { get; set; }
        public List<int> AlbumsIds { get; set; }
        public List<int> ConversationsIds { get; set; }
        public List<int> GroupsIds { get; set; }
        public List<int> PostsIds { get; set; }
        public List<int> CommentsIds { get; set; }
        public Bitmap ProfileImage { get; set; }
    }

    public class UserDBContext : DbContext
    {
        public UserDBContext() : base("DBConnectionString") { }
        public DbSet<User> Users { get; set; }
    }
}