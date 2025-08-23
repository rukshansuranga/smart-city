using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.Client;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    // ...removed [AllowAnonymous] to enforce authentication...
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        public ClientController(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(id); // Calls service to fetch product by ID
                return Ok(client); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Add(ClientPostRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            try
            {
                Client client = _mapper.Map<Client>(request);
                //client.CreatedDate = PSMDateTime.Now; // Set the created date to current time

                var createdClient = await _clientRepository.AddAsync(client);
                return CreatedAtAction(nameof(GetById), new { id = createdClient.ClientId }, createdClient);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //generate action for update project
        [HttpPut("{id}")]       
        public async Task<IActionResult> Update(string id, ClientPostRequest request)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var existingClient = await _clientRepository.GetByIdAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            _mapper.Map<ClientPostRequest, Client>(request, existingClient);

            var updatedClient = await _clientRepository.UpdateAsync(existingClient);
            return Ok(updatedClient);
        }
    }
}
