using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
public class Author{
        [Required]
        public string Name { get; set; }
        [Required]
        public string Discriminator { get; set; }
        [Required]
        public string Avatar { get; set; }
        [Required]
        public int MessageCount { get; set; }
        [Required]
        public int ActiveTime { get; set; }
        [Required]
        public int XP { get; set; }
        [Required]
        public List<Message> messages = new List<Message>();
}
