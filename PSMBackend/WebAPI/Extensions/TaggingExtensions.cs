using PSMWebAPI.Services;

namespace PSMWebAPI.Extensions;

/// <summary>
/// Extension methods to integrate tagging system with existing controllers
/// </summary>
public static class TaggingExtensions
{
    /// <summary>
    /// Helper method to assign tags to an entity after creation
    /// </summary>
    /// <param name="tagService">Tag service instance</param>
    /// <param name="entityType">Type of entity (Ticket, Complain, Project)</param>
    /// <param name="entityId">ID of the entity</param>
    /// <param name="tagIds">List of tag IDs to assign</param>
    /// <param name="assignedBy">User who assigned the tags</param>
    /// <returns>True if successful</returns>
    public static async Task<bool> AssignTagsAfterEntityCreationAsync(
        this ITagService tagService,
        string entityType, 
        int entityId, 
        List<int>? tagIds, 
        string? assignedBy = null)
    {
        if (tagIds == null || !tagIds.Any())
            return true; // No tags to assign, but not an error
            
        try
        {
            return await tagService.AssignTagsToEntityAsync(entityType, entityId, tagIds, assignedBy);
        }
        catch
        {
            // Log the error but don't fail the entity creation
            // You might want to add proper logging here
            return false;
        }
    }
}

/// <summary>
/// Example of how to integrate tags with existing ticket creation
/// This shows how you can modify your existing TicketController methods
/// </summary>
public static class TicketControllerTaggingExample
{
    /// <summary>
    /// Example showing how to modify the AddComplainTicket method to support tags
    /// This is just an example - you would integrate this into your actual controller
    /// </summary>
    public static string ExampleModifiedAddComplainTicketMethod => @"
[HttpPost(""complain"")]
public async Task<IActionResult> AddComplainTicket(TicketPostRequest request)
{
    try
    {
        // Your existing ticket creation logic
        var ticket = new ComplainTicket
        {
            Subject = request.Subject,
            Detail = request.Detail,
            Note = request.Note,
            Status = request.Status ?? TicketStatus.Open,
            UserId = request.UserId,
            CreatedBy = request.CreatedBy,
            Estimation = request.Estimation,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ComplainType = request.TicketComplainType,
            IsActive = true,
            CreatedAt = SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime
        };

        var result = await _ticketRepository.AddComplainTicketAsync(ticket, request.ComplainIdList);
        
        if (result != null)
        {
            // NEW: Assign tags after ticket creation
            if (request.TagIds?.Any() == true)
            {
                await _tagService.AssignTagsAfterEntityCreationAsync(
                    ""Ticket"", 
                    result.TicketId, 
                    request.TagIds, 
                    request.CreatedBy);
            }
            
            return Ok(ApiResponse<ComplainTicket>.Success(result, ""Ticket added successfully""));
        }
        
        return StatusCode(500, ApiResponse.Failure(""Failed to add ticket""));
    }
    catch (Exception ex)
    {
        return StatusCode(500, ApiResponse.Failure(""Error adding ticket"", ex.Message));
    }
}";

    /// <summary>
    /// Example showing how to include tags when retrieving tickets
    /// </summary>
    public static string ExampleGetTicketWithTags => @"
[HttpGet(""{id}"")]
public async Task<IActionResult> GetTicketWithTags(int id)
{
    try
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(ApiResponse<Ticket>.Failure(""Ticket not found""));

        // Get tags for the ticket
        var tags = await _tagService.GetEntityTagsAsync(""Ticket"", id);
        
        // You can either include tags in a response DTO or add them to the response
        var response = new
        {
            Ticket = ticket,
            Tags = tags
        };
        
        return Ok(ApiResponse<object>.Success(response, ""Ticket retrieved successfully""));
    }
    catch (Exception ex)
    {
        return StatusCode(500, ApiResponse.Failure(""Error retrieving ticket"", ex.Message));
    }
}";
}
