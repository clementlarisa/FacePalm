using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public List<int> UsersIds { get; set; }
        public List<int> PostsIds { get; set; }
    }

    public class GroupDBContext : DbContext
    {
        public GroupDBContext() : base("DBConnectionString") { }
        public DbSet<Group> Groups { get; set; }
    }
}