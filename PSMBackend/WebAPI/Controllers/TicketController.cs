using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;
using PSMWebAPI.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace PSMWebAPI.Controllers
{
        [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        public TicketController(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TicketRequest request)
        {
            var workPackageList = new List<TicketPackage>();
            foreach (var workPackageId in request.WorkpackageIdList)
            {
                workPackageList.Add(new TicketPackage
                {
                    WorkpackageId = workPackageId
                });
            }

            // var ticket = new Ticket
            // {
            //     Title = request.Title,
            //     Detail = request.Detail,
            //     CreatedDate = PSMDateTime.Now, // Uses the utility class to get current time in Colombo timezone
            //     UserId = request.UserId,
            //     Note = request.Note,
            //     TicketPackages = workPackageList,
            //     Type = request.Type,
            //     Status = request.Status
            // };

            var ticket = _mapper.Map<Ticket>(request); // Uses the utility class to get current time in Colombo timezone
            ticket.TicketPackages = workPackageList;


            var updatedTicket = await _ticketRepository.AddAsync(ticket); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedTicket.TicketId }, updatedTicket);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpPost("internal")]
        public async Task<IActionResult> AddInternal(TicketRequest request)
        {

            // var ticket = new Ticket
            // {
            //     Title = request.Title,
            //     Detail = request.Detail,
            //     CreatedDate = PSMDateTime.Now, // Uses the utility class to get current time in Colombo timezone
            //     UserId = request.UserId,
            //     Note = request.Note,
            //     Type = request.Type,
            //     Status = request.Status
            // };

            var ticket = _mapper.Map<Ticket>(request);

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

        [HttpGet("workpackage/{id}")]
        public async Task<IActionResult> GetTicketListByWorkpackageId(int id)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketListByWorkPackageIdAsync(id); // Calls service to fetch product by ID
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
            var response = await _ticketRepository.GetPagingAsync(paging); // Calls service to fetch product by ID
            return Ok(response); // Returns 200 OK response if found

        }

        [HttpPut("{ticketId}")]
        public async Task<IActionResult> Update(int ticketId, TicketRequest request)
        {
            var ticket = _mapper.Map<Ticket>(request);
            var existingTicket = await _ticketRepository.UpdateTicketHistoryAsync(ticket, ticket.UserId.Value);

            //var selectedTicket = await _ticketRepository.GetByIdAsync(ticketId);

            _mapper.Map<TicketRequest, Ticket>(request, existingTicket);

            var updatedTicket = await _ticketRepository.UpdateAsync(existingTicket); // Calls service to add a new product

            return Ok(updatedTicket);
        }
    }
}
