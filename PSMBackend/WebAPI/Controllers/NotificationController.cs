using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.Workpackage;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetById(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        [HttpPost]
        public async Task<ActionResult> Add(Notification notification)
        {
            await _notificationRepository.AddAsync(notification);
            return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Notification notification)
        {
            if (id != notification.Id)
                return BadRequest();
            await _notificationRepository.UpdateAsync(notification);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _notificationRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByClient(string clientId)
        {
            var notifications = await _notificationRepository.GetNotificationsByClientAsync(clientId);
            return Ok(notifications);
        }

        [HttpGet("read/{id}")]
        public async Task<ActionResult<Notification>> ReadNotification(int id)
        {
            await _notificationRepository.ReadNotificationAsync(id);

            return Ok();
        }

        [HttpPost("rating")]
        public async Task<IActionResult> AddRating(RatingRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            await _notificationRepository.AddRating(request);
            return Ok();
        }

        [HttpGet("unread/count/{clientId}")]
        public async Task<ActionResult<int>> GetUnreadNotificationCount(string clientId)
        {
            var count = await _notificationRepository.GetUnreadNotificationCountAsync(clientId);
            return Ok(count);
        }
    }
}
