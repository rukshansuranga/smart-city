using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;
using PSMWebAPI.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using PSMWebAPI.DTOs.Ticket;

namespace PSMWebAPI.Controllers
{
    // ...removed [AllowAnonymous] to enforce authentication...
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    // ...existing code...
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        public TicketController(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }


        [HttpPost("complain")]
        public async Task<IActionResult> AddComplainTicket(TicketPostRequest request)
        {
            var ComplainIdList = new List<TicketPackage>();
            foreach (var ComplainId in request.ComplainIdList)
            {
                ComplainIdList.Add(new TicketPackage
                {
                    ComplainId = ComplainId,
                });
            }

            var ticket = _mapper.Map<ComplainTicket>(request); // Uses the utility class to get current time in Colombo timezone
            ticket.TicketPackages = ComplainIdList;

            var addedTicket = await _ticketRepository.AddAsync(ticket); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = addedTicket.TicketId }, addedTicket);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpPost("internal")]
        public async Task<IActionResult> AddInternal(TicketPostRequest request)
        {
            var ticket = _mapper.Map<Ticket>(request);

            var updatedTicket = await _ticketRepository.AddAsync(ticket); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedTicket.TicketId }, updatedTicket);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicket(int ticketId)
        {
            var existingTicket = await _ticketRepository.GetByIdAsync<ComplainTicket>(ticketId);
            if (existingTicket == null)
            {
                return NotFound();
            }
            existingTicket.IsActive = false;
            await _ticketRepository.UpdateAsync(existingTicket);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync<ComplainTicket>(id); // Calls service to fetch product by ID
                return Ok(ticket); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet("complain/{id}")]
        public async Task<IActionResult> GetTicketListByComplainId(int id)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketListByComplainIdAsync(id); // Calls service to fetch product by ID
                return Ok(tickets); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TicketPaging paging)
        {
            var response = await _ticketRepository.GetPagingAsync<ComplainTicket>(paging); // Calls service to fetch product by ID
            return Ok(response); // Returns 200 OK response if found
        }

        [HttpPut("{ticketId}")]
        public async Task<IActionResult> Update(int ticketId, TicketUpdateRequest request)
        {
            var ticket = _mapper.Map<Ticket>(request);
            ticket.TicketId = ticketId;

            var existingTicket = await _ticketRepository.UpdateTicketHistoryAsync(ticket, ticket.UserId);

            //var selectedTicket = await _ticketRepository.GetByIdAsync(ticketId);

            _mapper.Map<TicketUpdateRequest, Ticket>(request, existingTicket);

            var updatedTicket = await _ticketRepository.UpdateAsync(existingTicket); // Calls service to add a new product

            return Ok(updatedTicket);
        }

        [HttpGet("start/{ticketId}")]
        public async Task<IActionResult> StartWorkOnTicket(int ticketId)
        {
            var result = await _ticketRepository.StartWorkOnTicketAsync(ticketId);
            if (!result) return NotFound();

            return Ok(result);
        }


        [HttpGet("resolve/{ticketId}")]
        public async Task<IActionResult> ResolveTicket(int ticketId)
        {
            var result = await _ticketRepository.ResolvedOnTicketAsync(ticketId);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpGet("close/{ticketId}")]
        public async Task<IActionResult> CloseTicket(int ticketId)
        {
            var result = await _ticketRepository.CloseOnTicketAsync(ticketId);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTicketListByUserIdAsync(string userId)
        {
            var result = await _ticketRepository.GetTicketListByUserIdAsync<ComplainTicket>(userId);
            return Ok(result);
        }

        [HttpGet("resolve")]
        public async Task<IActionResult> GetResolvedTickets()
        {
            var result = await _ticketRepository.GetResolvedTicketsAsync();
            return Ok(result);
        }

        [HttpPost("addcomplains")]
        public async Task<IActionResult> AddComplains(UpdateTicketPayload updateTicket)
        {
            await _ticketRepository.AddComplainsAsync(updateTicket.TicketId, updateTicket.ComplainIds);
            return Ok();
        }

        [HttpPost("removecomplains")]
        public async Task<IActionResult> RemoveComplains(UpdateTicketPayload updateTicket)
        {
            await _ticketRepository.RemoveComplainsAsync(updateTicket.TicketId, updateTicket.ComplainIds);
            return Ok();
        }
    }
}
