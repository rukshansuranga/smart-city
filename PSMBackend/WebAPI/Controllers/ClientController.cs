using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.Client;
using PSMWebAPI.DTOs.Common;
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
                if (client == null)
                {
                    return NotFound(ApiResponse<Client>.Failure("Client not found"));
                }
                return Ok(ApiResponse<Client>.Success(client, "Client retrieved successfully")); // Returns 200 OK response if found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving client", ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Add(ClientPostRequest request)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse<Client>.Failure("Request cannot be null"));
            }

            try
            {
                Client client = _mapper.Map<Client>(request);
                //client.CreatedDate = PSMDateTime.Now; // Set the created date to current time

                var createdClient = await _clientRepository.AddAsync(client);
                return CreatedAtAction(nameof(GetById), new { id = createdClient.ClientId }, 
                    ApiResponse<Client>.Success(createdClient, "Client created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error creating client", ex.Message));
            }
        }

        //generate action for update project
        [HttpPut("{id}")]       
        public async Task<IActionResult> Update(string id, ClientPostRequest request)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ApiResponse<Client>.Failure("Client ID cannot be null or empty"));
            }

            try
            {
                var existingClient = await _clientRepository.GetByIdAsync(id);
                if (existingClient == null)
                {
                    return NotFound(ApiResponse<Client>.Failure("Client not found"));
                }

                _mapper.Map<ClientPostRequest, Client>(request, existingClient);

                var updatedClient = await _clientRepository.UpdateAsync(existingClient);
                return Ok(ApiResponse<Client>.Success(updatedClient, "Client updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error updating client", ex.Message));
            }
        }
    }
}
