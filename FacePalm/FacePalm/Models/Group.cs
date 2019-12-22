﻿using System;
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
        public string GroupId { get; set; }
        public List<string> UsersIds { get; set; }
        public List<int> PostsIds { get; set; }
    }
    
}