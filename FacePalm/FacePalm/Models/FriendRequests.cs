using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacePalm.Models
{
    public class FriendRequest
    {
        
        public string FromUserId { get; set; }
        
        public string ToUserId { get; set; }
    }
}