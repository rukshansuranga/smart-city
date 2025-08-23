using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
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
        public async Task<ActionResult<IEnumerable<TicketActivity>>> GetAll()
        {
            var activities = await _ticketActivityRepository.GetAllAsync();
            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketActivity>> GetById(int id)
        {
            var activity = await _ticketActivityRepository.GetByIdAsync(id);
            if (activity == null)
                return NotFound();
            return Ok(activity);
        }

        [HttpPost]
        public async Task<ActionResult> Add(TicketActivity activity)
        {
            await _ticketActivityRepository.AddAsync(activity);
            return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TicketActivity activity)
        {
            if (id != activity.Id)
                return BadRequest();
            await _ticketActivityRepository.UpdateAsync(activity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _ticketActivityRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
