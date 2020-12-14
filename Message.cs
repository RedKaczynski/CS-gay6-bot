using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace channel_scraper
{
    public class Message
    {
        [Required, Key]
        public ulong Id { get; set; }
        [Required]
        public string Channel { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public string Content { get; set; }
    }
    public class Author{
        [Required, Key]
        public ulong Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ulong Discriminator { get; set; }
        public int MessageCount = 0;
        public int ActiveTime = 0;
        public int XP = 0;

        public List<Message> dinosaurs = new List<Message>();
    }
}