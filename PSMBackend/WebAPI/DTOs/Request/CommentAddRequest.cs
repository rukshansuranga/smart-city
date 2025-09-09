using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Request;

public class CommentAddRequest
{
    public EntityType EntityType { get; set; }
    public required string EntityId { get; set; }
    public required string Text { get; set; }
    public CommentType Type { get; set; }    
    public bool IsPrivate { get; set; }
    public string? ClientId { get; set; }
    
}
