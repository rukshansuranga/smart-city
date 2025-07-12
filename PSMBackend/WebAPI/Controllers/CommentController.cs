using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
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
            var comment = new Comment
            {
                Text = request.Comment,
                WorkpackageId = request.WorkPackageId,
                CreatedDate = PSMDateTime.Now,
                UpdatedDate = PSMDateTime.Now,
                IsPrivate = request.IsPrivate,
                Type = request.Type,
                ClientId = request.ClientId
            };

            var updatedComment = await _commentRepository.AddAsync(comment); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedComment.CommentId }, updatedComment);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _commentRepository.GetByIdAsync(id); // Calls service to fetch product by ID
                return Ok(ticket); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }
    }
}
