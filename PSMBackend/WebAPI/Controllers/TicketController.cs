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
            try
            {
                var ticket = _mapper.Map<ComplainTicket>(request); // Uses the utility class to get current time in Colombo timezone
                
                // Create the ticket first without the complain relationships
                var addedTicket = await _ticketRepository.AddAsync(ticket); // Calls service to add a new product
                
                // Now add the complains to the existing ticket using the repository method
                if (request?.ComplainIdList != null && request.ComplainIdList.Any())
                {
                    await _ticketRepository.AddComplainsAsync(addedTicket.TicketId, request.ComplainIdList);
                }
                
                return CreatedAtAction(nameof(GetById), new { id = addedTicket.TicketId }, 
                    ApiResponse<ComplainTicket>.Success(addedTicket, "Complain ticket created successfully"));
                // Returns 201 Created response with location header pointing to the new product
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error creating complain ticket", ex.Message));
            }
        }

        [HttpPost("internal")]
        public async Task<IActionResult> AddInternal(TicketPostRequest request)
        {
            try
            {
                var ticket = _mapper.Map<InternalTicket>(request);

                var updatedTicket = await _ticketRepository.AddAsync<InternalTicket>(ticket); // Calls service to add a new product
                return CreatedAtAction(nameof(GetById), new { id = updatedTicket.TicketId }, 
                    ApiResponse<Ticket>.Success(updatedTicket, "Internal ticket created successfully"));
                // Returns 201 Created response with location header pointing to the new product
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error creating internal ticket", ex.Message));
            }
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicket(int ticketId)
        {
            try
            {
                var existingTicket = await _ticketRepository.GetByIdAsync<ComplainTicket>(ticketId);
                if (existingTicket == null)
                {
                    return NotFound(ApiResponse<ComplainTicket>.Failure($"Ticket with ID {ticketId} not found"));
                }
                existingTicket.IsActive = false;
                await _ticketRepository.UpdateAsync(existingTicket);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error deleting ticket", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync<Ticket>(id); // Calls service to fetch product by ID
                if (ticket == null)
                {
                    return NotFound(ApiResponse<Ticket>.Failure("Ticket not found"));
                }
                return Ok(ApiResponse<Ticket>.Success(ticket, "Ticket retrieved successfully")); // Returns 200 OK response if found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving ticket", ex.Message));
            }
        }

        [HttpGet("complain/{id}")]
        public async Task<IActionResult> GetTicketListByComplainId(int id)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketListByComplainIdAsync(id); // Calls service to fetch product by ID
                return Ok(ApiResponse<IEnumerable<ComplainTicket>>.Success(tickets, "Tickets retrieved successfully")); // Returns 200 OK response if found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving tickets by complain ID", ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TicketPaging paging)
        {
            try
            {
                var response = await _ticketRepository.GetPagingAsync<Ticket>(paging); // Calls service to fetch product by ID
                return Ok(ApiResponse<object>.Success(response, "Tickets retrieved successfully")); // Returns 200 OK response if found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving tickets", ex.Message));
            }
        }

        [HttpPut("{ticketId}")]
        public async Task<IActionResult> Update(int ticketId, TicketUpdateRequest request)
        {
            try
            {
                var ticket = _mapper.Map<Ticket>(request);
                ticket.TicketId = ticketId;

                var existingTicket = await _ticketRepository.UpdateTicketHistoryAsync(ticket, ticket.UserId ?? "");

                //var selectedTicket = await _ticketRepository.GetByIdAsync(ticketId);

                _mapper.Map<TicketUpdateRequest, Ticket>(request, existingTicket);

                var updatedTicket = await _ticketRepository.UpdateAsync(existingTicket); // Calls service to add a new product

                return Ok(ApiResponse<Ticket>.Success(updatedTicket, "Ticket updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error updating ticket", ex.Message));
            }
        }

        [HttpGet("start/{ticketId}")]
        public async Task<IActionResult> StartWorkOnTicket(int ticketId)
        {
            try
            {
                var result = await _ticketRepository.StartWorkOnTicketAsync(ticketId);
                if (!result) return NotFound(ApiResponse<bool>.Failure($"Ticket with ID {ticketId} not found"));

                return Ok(ApiResponse<bool>.Success(result, "Work started on ticket successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error starting work on ticket", ex.Message));
            }
        }


        [HttpGet("resolve/{ticketId}")]
        public async Task<IActionResult> ResolveTicket(int ticketId)
        {
            try
            {
                var result = await _ticketRepository.ResolvedOnTicketAsync(ticketId);
                if (!result) return NotFound(ApiResponse<bool>.Failure($"Ticket with ID {ticketId} not found"));

                return Ok(ApiResponse<bool>.Success(result, "Ticket resolved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error resolving ticket", ex.Message));
            }
        }

        [HttpGet("close/{ticketId}")]
        public async Task<IActionResult> CloseTicket(int ticketId)
        {
            try
            {
                var result = await _ticketRepository.CloseOnTicketAsync(ticketId);
                if (!result) return NotFound(ApiResponse<bool>.Failure($"Ticket with ID {ticketId} not found"));

                return Ok(ApiResponse<bool>.Success(result, "Ticket closed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error closing ticket", ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTicketListByUserIdAsync(string userId)
        {
            try
            {
                var result = await _ticketRepository.GetTicketListByUserIdAsync(userId);
                return Ok(ApiResponse<BoardTicket>.Success(result, "Tickets retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving tickets by user ID", ex.Message));
            }
        }

        [HttpGet("resolve")]
        public async Task<IActionResult> GetResolvedTickets()
        {
            try
            {
                var result = await _ticketRepository.GetResolvedTicketsAsync();
                return Ok(ApiResponse<IEnumerable<Ticket>>.Success(result, "Resolved tickets retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving resolved tickets", ex.Message));
            }
        }

        [HttpPost("addcomplains")]
        public async Task<IActionResult> AddComplains(UpdateTicketPayload updateTicket)
        {
            try
            {
                await _ticketRepository.AddComplainsAsync(updateTicket.TicketId, updateTicket.ComplainIds);
                return Ok(ApiResponse.Success("Complains added to ticket successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error adding complains to ticket", ex.Message));
            }
        }

        [HttpPost("removecomplains")]
        public async Task<IActionResult> RemoveComplains(UpdateTicketPayload updateTicket)
        {
            try
            {
                await _ticketRepository.RemoveComplainsAsync(updateTicket.TicketId, updateTicket.ComplainIds);
                return Ok(ApiResponse.Success("Complains removed from ticket successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error removing complains from ticket", ex.Message));
            }
        }
    }
}
