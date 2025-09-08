using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;

namespace PSMModel.Models;

public class Comment : BaseEntity
{
    public int CommentId { get; set; }
    public required string Text { get; set; }

    public EntityType EntityType { get; set; }
    public required string EntityId { get; set; }
    public bool? IsPrivate { get; set; }
    
    [ForeignKey("Client")]
    public string? ClientId { get; set; }
    public Client? Client { get; set; }
    
    [ForeignKey("User")]
    public string? UserId { get; set; }
    public User? User { get; set; }
    
    public CommentType? Type { get; set; }
}
