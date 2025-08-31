using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class Comment : BaseEntity
{
public int CommentId { get; set; }
public string Text { get; set; }

[ForeignKey("Complain")]
public int ComplainId { get; set; }
public Complain? Complain { get; set; }
public bool? IsPrivate { get; set; }
[ForeignKey("Client")]
public string? ClientId { get; set; }
public Client? Client { get; set; }
[ForeignKey("User")]
public string? UserId { get; set; }
public User? User { get; set; }
public string? Type { get; set; }
}
