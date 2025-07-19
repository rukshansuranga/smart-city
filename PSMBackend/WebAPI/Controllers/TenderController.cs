using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.tender;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderRepository _tenderRepository;
        private readonly IMapper _mapper;
        public TenderController(ITenderRepository tenderRepository, IMapper mapper)
        {
            _tenderRepository = tenderRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _tenderRepository.GetByIdAsync(id); // Calls service to fetch product by ID
                return Ok(ticket); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet("project/{projectId}")]
        // [Route("project")]
        public async Task<IActionResult> Get(int projectId)
        {
            if (projectId <= 0)
            {
                return BadRequest("Invalid project ID.");
            }

            var response = await _tenderRepository.GetTendersByProjectIdAsync(projectId); // Calls service to fetch product by ID
            return Ok(response); // Returns 200 OK response if found
        }
        
        //generate action for create project
        [HttpPost]
        public async Task<IActionResult> Add(TenderPostRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            try
            {
                Tender tender = _mapper.Map<Tender>(request);
            tender.SubmittedDate = PSMDateTime.Now; // Set the submitted date to current time

            var createdTender = await _tenderRepository.AddAsync(tender);
            return CreatedAtAction(nameof(GetById), new { id = createdTender.Id }, createdTender);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //generate action for update project
        [HttpPut("{id}")]       
        public async Task<IActionResult> Update(int id, TenderPostRequest request)
        {
            if (request == null || id <= 0)
            {
                return BadRequest();
            }

            var existingTender = await _tenderRepository.GetByIdAsync(id);
            if (existingTender == null)
            {
                return NotFound();
            }

            _mapper.Map<TenderPostRequest,Tender>(request,existingTender);
            
            //_mapper.Map<Project, Project>(mapObject, existingProject);

            var updatedTender = await _tenderRepository.UpdateAsync(existingTender);
            return Ok(updatedTender);
        }

    }
}
