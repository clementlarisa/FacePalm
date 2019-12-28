﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacePalm.Models
{
    public class Conversation
    {
        [Key]
        public string ConversationId { get; set; }
        [Required]
        public string FirstUserId { get; set; }
        [Required]
        public string SecondUserId { get; set; }
        public List<string> MessagesIds { get; set; }
        public DateTime Date { get; set; }
    }

}