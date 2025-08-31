using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Complain;
using PSMWebAPI.DTOs.Complain.GeneralComplain;
using PSMWebAPI.DTOs.Complain.ProjectComplain;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
    // [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainController : ControllerBase
    {
        private readonly IComplainRepository _complainRepository; // Repository instance for database operations
        private readonly IMapper _mapper;
        public ComplainController(IComplainRepository complainRepository, IMapper mapper)
        {
            _complainRepository = complainRepository;
            _mapper = mapper;
        }
        //Handles HTTP POST request to add a new product
        [HttpPost]
        public async Task<IActionResult> AddComplain(ComplainRequest request)
        {

            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var complain = _mapper.Map<Complain>(request);
            complain.Status = request.Status ?? ComplainStatus.New; // Default status if not provided
            var updatedComplain = await _complainRepository.AddComplainAsync(complain); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedComplain.ComplainId }, updatedComplain);
            // Returns 201 Created response with location header pointing to the new product
        }


        [HttpGet("Complain/{ComplainId}")]
        public async Task<IActionResult> GetByComplainId(int ComplainId)
        {
            try
            {
                var complain = await _complainRepository.GetByIdAsync<Complain>(ComplainId);
                return Ok(complain); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        

        [HttpPost("lightPost")]
        public async Task<IActionResult> AddComplain(LightPostComplainRequest request)
        {
            var complain = _mapper.Map<LightPostComplain>(request);
            complain.Status = request.Status ?? ComplainStatus.New;

            var updatedComplain = await _complainRepository.AddComplainAsync(complain); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedComplain.ComplainId }, updatedComplain);
            // Returns 201 Created response with location header pointing to the new product
        }

        // Handles HTTP GET request to fetch a single product by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var complain = await _complainRepository.GetByIdAsync<Complain>(id); // Calls service to fetch product by ID
                return Ok(complain); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetComplains([FromQuery] ComplainPaging complainPaging)
        {
            var complains = await _complainRepository.GetComplains(complainPaging);
            return Ok(complains); // Returns 200 OK response with the list of work packages
        }

        [HttpGet]
        [Route("ticket/{ticketId}")]
        public async Task<IActionResult> GetComplainByTicketId(int ticketId)
        {
            try
            {
                var complains = await _complainRepository.GetComplainsByTicketId(ticketId); // Calls service to fetch product by ID
                return Ok(complains); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpPost]
        [Route("ticket")]
        //[Route("ticket/{ComplainId}/{ticketId}")]
        public async Task<IActionResult> ManageWorkpackateTicketMapping(TicketPackageRequest request)
        {
            try
            {
                if (request.Action == "Add")
                {
                    await _complainRepository.AddComplainMappingByTicketId(request.TicketId, request.ComplainId); // Calls service to fetch product by ID
                    return Ok(); // Returns 200 OK response if found
                }
                else
                {
                    await _complainRepository.DeleteComplainMappingByTicketId(request.TicketId, request.ComplainId);
                    return Ok();
                }

            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        [Route("lightPost/{postNo}")]
        public async Task<IActionResult> GetSummaryPostComplainsByPostNo(string postNo)
        {
            try
            {

                var complainsByPostNo = await _complainRepository.GetSummaryLightPostComplintsByPostId(postNo);
                return Ok(complainsByPostNo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        [Route("lightPost/near")]
        public async Task<IActionResult> GetNearLightPost([FromQuery] LocationPoint point)
        {
            try
            {

                var complainsByPostNo = await _complainRepository.GetLightPostsByCenterPoint(point.Latitude, point.Longitude);
                return Ok(complainsByPostNo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        [Route("lightPost/{postNo}/{name}")]
        public async Task<IActionResult> GetDetailPostComplainsByPostNoAndName(string postNo, string name)
        {
            try
            {
                var complainsByPostNo = await _complainRepository.GetDetailLightPostComplintsByPostIdAndName(postNo, name, "");
                return Ok(complainsByPostNo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }


        [HttpGet("lightpostcomplain/{ComplainId}")]
        public async Task<IActionResult> GetLightPostByComplainId(int ComplainId)
        {
            try
            {
                var complain = await _complainRepository.GetByIdAsync<LightPostComplain>(ComplainId);
                return Ok(complain); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet("lightpost/me/{postNo}")]
        public async Task<IActionResult> GetLightPostByComplainId(string postNo)
        {
            
                var myActiveComplains = await _complainRepository.GetLightPostActiveComplainsByMe(postNo);
                return Ok(myActiveComplains); // Returns 200 OK response if found
        }

        [HttpGet("lightpost/active")]
        public async Task<IActionResult> GetActiveLightPostMarkers()
        {
            var activeMarkers = await _complainRepository.GetActiveLightPostList();
            return Ok(activeMarkers);
        } 
    
        [HttpGet("lightpost/assigned")]
        public async Task<IActionResult> GetActiveAndAssignedLightPostMarkers()
        {
            var activeMarkers = await _complainRepository.GetActiveAndAssignedLightPostList();
            return Ok(activeMarkers);
        }  

        [HttpPost("lightpost/getpointincircle")]
        public async Task<IActionResult> GetLightPostsInCircle([FromBody] CircleRequest request)
        {
            var activeMarkers = await _complainRepository.GetLightPostListByLocation(request.Latitude, request.Longitude, request.Statuses);
            return Ok(activeMarkers);
        }  

        #region General Complain

        [HttpDelete("general/{complainId}")]
        public async Task<IActionResult> DeleteGeneralComplain(int complainId)
        {
            var existingComplain = await _complainRepository.GetByIdAsync<GeneralComplain>(complainId);
            if (existingComplain == null)
            {
                return NotFound();
            }
            existingComplain.IsActive = false;
            await _complainRepository.UpdateComplainAsync<GeneralComplain>(existingComplain);
            return NoContent();
        }

        [HttpPost("general")]
        public async Task<IActionResult> AddGeneralComplain(GeneralComplainAddRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }
            var generalComplain = _mapper.Map<GeneralComplain>(request);
            generalComplain.Status = request.Status ?? ComplainStatus.New; // Default status if not provided


            var addedComplain = await _complainRepository.AddComplainAsync<GeneralComplain>(generalComplain); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = addedComplain.ComplainId }, addedComplain);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpPut("general/{complainId}")]
        public async Task<IActionResult> UpdateGeneralComplain(int complainId, GeneralComplainUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var generalComplain = _mapper.Map<GeneralComplain>(request);
            generalComplain.ComplainId = complainId; // Ensure the ID is set for the

            var existingComplain = await _complainRepository.GetByIdAsync<GeneralComplain>(complainId);
            if (existingComplain == null)
            {
                return NotFound();
            }

            _mapper.Map(generalComplain, existingComplain);

            var updatedComplain = await _complainRepository.UpdateComplainAsync<GeneralComplain>(existingComplain);
            return Ok(updatedComplain);
        }

        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralComplainsPaging([FromQuery] GeneralComplainGetPagingRequest complainPaging)
         {
            var generalComplains = await _complainRepository.GetGeneralComplain(complainPaging);
            return Ok(generalComplains); // Returns 200 OK response with the list of work packages
        }

        [HttpGet("generalcomplain/{ComplainId}")]
        public async Task<IActionResult> GetGeneralComplainByComplainId(int ComplainId)
        {
            try
            {
                var generalComplain = await _complainRepository.GetByIdAsync<GeneralComplain>(ComplainId);
                return Ok(generalComplain); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        #endregion

        #region Project Complain

        [HttpGet("projectcomplains/{projectId}")]
        public async Task<IActionResult> GetProjectComplainsByProjectId(int projectId)
        {
            try
            {
                var projectComplains = await _complainRepository.GetProjectComplainsByProjectId(projectId);
                return Ok(projectComplains); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpPost("projectcomplain")]
        public async Task<IActionResult> AddProjectComplain(ProjectComplainPostRequest request)
        {

            var projectComplain = _mapper.Map<ProjectComplain>(request);
            projectComplain.Status = ComplainStatus.New; // Default status if not provided

            var createdProjectComplain = await _complainRepository.AddComplainAsync<ProjectComplain>(projectComplain);
            return CreatedAtAction(nameof(GetProjectComplainsByProjectId), new { projectId = createdProjectComplain.ProjectId }, createdProjectComplain);
        }

        [HttpPut("projectcomplain/{complainId}")]
        public async Task<IActionResult> UpdateProjectComplain(int complainId, ProjectComplainUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var projectComplain = _mapper.Map<ProjectComplain>(request);
            projectComplain.ComplainId = complainId; // Ensure the ID is set for the

            var existingProjectComplain = await _complainRepository.GetByIdAsync<ProjectComplain>(complainId);
            if (existingProjectComplain == null)
            {
                return NotFound();
            }

            _mapper.Map(projectComplain, existingProjectComplain);

            var updatedComplain = await _complainRepository.UpdateComplainAsync<ProjectComplain>(existingProjectComplain);
            return Ok(updatedComplain);
        }

        [HttpGet("projectcomplain/{ComplainId}")]
        public async Task<IActionResult> GetProjectComplainByComplainId(int ComplainId)
        {
            try
            {
                var projectComplain = await _complainRepository.GetByIdAsync<ProjectComplain>(ComplainId);
                return Ok(projectComplain); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpDelete("projectcomplain/{complainId}")]
        public async Task<IActionResult> DeleteProjectComplain(int complainId)
        {
            var existingComplain = await _complainRepository.GetByIdAsync<ProjectComplain>(complainId);
            if (existingComplain == null)
            {
                return NotFound();
            }
            existingComplain.IsActive = false;
            await _complainRepository.UpdateComplainAsync<ProjectComplain>(existingComplain);
            return NoContent();
        }

        #endregion


    }
}
