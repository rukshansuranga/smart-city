using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Comment;

public class CommentGetByEntityRequest
{
    public EntityType EntityType { get; set; }
    public required string EntityId { get; set; }
}
