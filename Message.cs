using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Message
{
    [Required, Key]
    public ulong Id { get; set; }
    [Required]
    public string Channel { get; set; }
    [Required]
    public long Time { get; set; }
    [Required]
    public string Content { get; set; }
}
