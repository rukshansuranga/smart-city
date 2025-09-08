using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.DTOs.Complain;
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
        public async Task<ActionResult<ApiResponse<IEnumerable<Notification>>>> GetAll()
        {
            try
            {
                var notifications = await _notificationRepository.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<Notification>>.Success(notifications, "Notifications retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Notification>>.Failure($"Failed to retrieve notifications: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Notification>>> GetById(int id)
        {
            try
            {
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                    return NotFound(ApiResponse<Notification>.Failure("Notification not found"));
                return Ok(ApiResponse<Notification>.Success(notification, "Notification retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Notification>.Failure($"Failed to retrieve notification: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Notification>>> Add(Notification notification)
        {
            try
            {
                await _notificationRepository.AddAsync(notification);
                return Ok(ApiResponse<Notification>.Success(notification, "Notification added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Notification>.Failure($"Failed to add notification: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, Notification notification)
        {
            try
            {
                if (id != notification.Id)
                    return BadRequest(ApiResponse<string>.Failure("Invalid notification ID"));
                await _notificationRepository.UpdateAsync(notification);
                return Ok(ApiResponse<string>.Success("", "Notification updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"Failed to update notification: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                await _notificationRepository.DeleteAsync(id);
                return Ok(ApiResponse<string>.Success("", "Notification deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"Failed to delete notification: {ex.Message}"));
            }
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Notification>>>> GetNotificationsByClient(string clientId)
        {
            try
            {
                var notifications = await _notificationRepository.GetNotificationsByClientAsync(clientId);
                return Ok(ApiResponse<IEnumerable<Notification>>.Success(notifications, "Client notifications retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Notification>>.Failure($"Failed to retrieve client notifications: {ex.Message}"));
            }
        }

        [HttpGet("read/{id}")]
        public async Task<ActionResult<ApiResponse<string>>> ReadNotification(int id)
        {
            try
            {
                await _notificationRepository.ReadNotificationAsync(id);
                return Ok(ApiResponse<string>.Success("", "Notification marked as read successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"Failed to mark notification as read: {ex.Message}"));
            }
        }

        [HttpPost("rating")]
        public async Task<IActionResult> AddRating(RatingRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(ApiResponse<string>.Failure("Request cannot be null"));
                }

                await _notificationRepository.AddRating(request);
                return Ok(ApiResponse<string>.Success("", "Rating added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"Failed to add rating: {ex.Message}"));
            }
        }

        [HttpGet("unread/count/{clientId}")]
        public async Task<ActionResult<ApiResponse<int>>> GetUnreadNotificationCount(string clientId)
        {
            try
            {
                var count = await _notificationRepository.GetUnreadNotificationCountAsync(clientId);
                return Ok(ApiResponse<int>.Success(count, "Unread notification count retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<int>.Failure($"Failed to retrieve unread notification count: {ex.Message}"));
            }
        }
    }
}
