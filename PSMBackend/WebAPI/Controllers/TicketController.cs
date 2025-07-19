using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;
using PSMWebAPI.DTOs.Request;
using Microsoft.AspNetCore.Authorization;

namespace PSMWebAPI.Controllers
{
        [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TicketRequest request)
        {
            var workPackageList = new List<TicketPackage>();
            foreach (var workPackageId in request.WorkpackageIdList)
            {
                workPackageList.Add(new TicketPackage
                {
                    WorkPackageId = workPackageId
                });
            }

            var ticket = new Ticket
            {
                Title = request.Title,
                Detail = request.Detail,
                CreatedDate = PSMDateTime.Now, // Uses the utility class to get current time in Colombo timezone
                UserId = request.UserId,
                Note = request.Note,
                TicketPackages = workPackageList
            };


            var updatedTicket = await _ticketRepository.AddAsync(ticket); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedTicket.TicketId }, updatedTicket);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(id); // Calls service to fetch product by ID
                return Ok(ticket); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TicketPaging paging)
        {
            var response = await _ticketRepository.GetPagingAsync(paging); // Calls service to fetch product by ID
            return Ok(response); // Returns 200 OK response if found

        }

        [HttpPut("{ticketId:int}")]
        public async Task<IActionResult> Update(int ticketId, TicketRequest request)
        {
            var selectedTicket = await _ticketRepository.GetByIdAsync(ticketId);

            selectedTicket.Title = request.Title;
            selectedTicket.Detail = request.Detail;
            selectedTicket.Note = request.Note;
            selectedTicket.UserId = request.UserId;

            var updatedTicket = await _ticketRepository.UpdateAsync(selectedTicket); // Calls service to add a new product

            return Ok(updatedTicket);
        }
    }
}
