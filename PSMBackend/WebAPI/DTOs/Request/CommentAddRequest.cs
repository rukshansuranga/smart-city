using System;

namespace PSMWebAPI.DTOs.Request;

public class CommentAddRequest
{
    public int WorkPackageId { get; set; }
    public string text { get; set; }
    public string Type { get; set; }    
    public bool IsPrivate { get; set; }
    public string ClientId { get; set; }
}
