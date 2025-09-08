using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Comment;

public class CommentUpdateRequest
{
    public int CommentId { get; set; }
    public required string Text { get; set; }
    public CommentType? Type { get; set; }
    public bool? IsPrivate { get; set; }
}
