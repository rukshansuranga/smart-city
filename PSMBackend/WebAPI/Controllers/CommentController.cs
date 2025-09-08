using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMModel.Enums;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Comment;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
    // ...removed [AllowAnonymous] to enforce authentication...
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Add(CommentAddRequest request)
        {
            try
            {
                var comment = new Comment
                {
                    Text = request.Text,
                    EntityType = request.EntityType,
                    EntityId = request.EntityId,
                    IsPrivate = request.IsPrivate,
                    Type = request.Type,
                    ClientId = request.ClientId
                };

                var updatedComment = await _commentRepository.AddAsync(comment);
                return Ok(ApiResponse<Comment>.Success(updatedComment, "Comment added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Comment>.Failure($"Failed to add comment: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var comment = await _commentRepository.GetByIdAsync(id);
                if (comment == null)
                {
                    return NotFound(ApiResponse<Comment>.Failure("Comment not found"));
                }
                return Ok(ApiResponse<Comment>.Success(comment, "Comment retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Comment>.Failure($"Failed to retrieve comment: {ex.Message}"));
            }
        }

        [HttpPost("entity")]
        public async Task<IActionResult> GetByEntity(CommentGetByEntityRequest request)
        {
            try
            {
                var comments = await _commentRepository.GetByEntityAsync(request.EntityType, request.EntityId);
                return Ok(ApiResponse<IEnumerable<Comment>>.Success(comments, "Comments retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Comment>>.Failure($"Failed to retrieve comments: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CommentUpdateRequest request)
        {
            try
            {
                if (id != request.CommentId)
                {
                    return BadRequest(ApiResponse<Comment>.Failure("Comment ID in URL doesn't match request body"));
                }

                // First check if comment exists
                var existingComment = await _commentRepository.GetByIdAsync(id);
                if (existingComment == null)
                {
                    return NotFound(ApiResponse<Comment>.Failure("Comment not found"));
                }

                // Create comment object for update
                var commentToUpdate = new Comment
                {
                    CommentId = request.CommentId,
                    Text = request.Text,
                    Type = request.Type,
                    IsPrivate = request.IsPrivate,
                    EntityType = existingComment.EntityType,
                    EntityId = existingComment.EntityId,
                    ClientId = existingComment.ClientId
                };

                var updatedComment = await _commentRepository.UpdateAsync(commentToUpdate);
                if (updatedComment == null)
                {
                    return NotFound(ApiResponse<Comment>.Failure("Comment not found"));
                }

                return Ok(ApiResponse<Comment>.Success(updatedComment, "Comment updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Comment>.Failure($"Failed to update comment: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _commentRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse<string>.Failure("Comment not found"));
                }

                return Ok(ApiResponse<string>.Success("Comment deleted successfully", "Comment deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"Failed to delete comment: {ex.Message}"));
            }
        }
    }
}
