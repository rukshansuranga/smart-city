using System;

namespace PSMWebAPI.DTOs.Request;

public class CommentAddRequest
{
    public int ComplainId { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }    
    public bool IsPrivate { get; set; }
    public string ClientId { get; set; }
}
