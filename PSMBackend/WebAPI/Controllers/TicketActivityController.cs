using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketActivityController : ControllerBase
    {
        private readonly ITicketActivityRepository _ticketActivityRepository;

        public TicketActivityController(ITicketActivityRepository ticketActivityRepository)
        {
            _ticketActivityRepository = ticketActivityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TicketActivity>>>> GetAll()
        {
            try
            {
                var activities = await _ticketActivityRepository.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<TicketActivity>>.Success(activities, "Ticket activities retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<TicketActivity>>.Failure($"Failed to retrieve ticket activities: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TicketActivity>>> GetById(int id)
        {
            try
            {
                var activity = await _ticketActivityRepository.GetByIdAsync(id);
                if (activity == null)
                    return NotFound(ApiResponse<TicketActivity>.Failure("Ticket activity not found"));
                return Ok(ApiResponse<TicketActivity>.Success(activity, "Ticket activity retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TicketActivity>.Failure($"Failed to retrieve ticket activity: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TicketActivity>>> Add(TicketActivity activity)
        {
            try
            {
                await _ticketActivityRepository.AddAsync(activity);
                return Ok(ApiResponse<TicketActivity>.Success(activity, "Ticket activity added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TicketActivity>.Failure($"Failed to add ticket activity: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, TicketActivity activity)
        {
            try
            {
                if (id != activity.Id)
                    return BadRequest(ApiResponse<string>.Failure("Invalid ticket activity ID"));
                await _ticketActivityRepository.UpdateAsync(activity);
                return Ok(ApiResponse<string>.Success("", "Ticket activity updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"Failed to update ticket activity: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                await _ticketActivityRepository.DeleteAsync(id);
                return Ok(ApiResponse<string>.Success("", "Ticket activity deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Failure($"Failed to delete ticket activity: {ex.Message}"));
            }
        }
    }
}
