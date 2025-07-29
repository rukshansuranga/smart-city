using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class Comment : BaseEntity
{
public int CommentId { get; set; }
public string Text { get; set; }

[ForeignKey("Workpackage")]
public int WorkpackageId { get; set; }
public Workpackage? Workpackage { get; set; }
public bool? IsPrivate { get; set; }
[ForeignKey("Client")]
public int? ClientId { get; set; }
public Client? Client { get; set; }
[ForeignKey("User")]
public int? UserId { get; set; }
public User? User { get; set; }
public string? Type { get; set; }
}
