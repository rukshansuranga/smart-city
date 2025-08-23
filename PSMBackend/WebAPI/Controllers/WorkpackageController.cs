using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Workpackage;
using PSMWebAPI.DTOs.Workpackage.GeneralComplain;
using PSMWebAPI.DTOs.Workpackage.ProjectComplain;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
    // [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkpackageController : ControllerBase
    {
        private readonly IWorkpackageRepository _workpackageRepository; // Repository instance for database operations
        private readonly IMapper _mapper;
        public WorkpackageController(IWorkpackageRepository workpackageRepository, IMapper mapper)
        {
            _workpackageRepository = workpackageRepository;
            _mapper = mapper;
        }
        //Handles HTTP POST request to add a new product
        [HttpPost]
        public async Task<IActionResult> Add(WorkpackageRequest request)
        {

            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var workpackage = _mapper.Map<Workpackage>(request);
            workpackage.Status = request.Status ?? WorkpackageStatus.New; // Default status if not provided
            var updatedWorkpackage = await _workpackageRepository.AddWorkpackageAsync(workpackage); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedWorkpackage.WorkpackageId }, updatedWorkpackage);
            // Returns 201 Created response with location header pointing to the new product
        }


        [HttpGet("Workpackage/{workPackageId}")]
        public async Task<IActionResult> GetByWorkPackageId(int workPackageId)
        {
            try
            {
                var workpackage = await _workpackageRepository.GetByIdAsync<Workpackage>(workPackageId);
                return Ok(workpackage); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        

        [HttpPost("lightPost")]
        public async Task<IActionResult> AddComplain(LightPostComplainRequest request)
        {
            var workpackage = _mapper.Map<LightPostComplain>(request);
            workpackage.Status = request.Status ?? WorkpackageStatus.New;

            var updatedWorkpackage = await _workpackageRepository.AddWorkpackageAsync(workpackage); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedWorkpackage.WorkpackageId }, updatedWorkpackage);
            // Returns 201 Created response with location header pointing to the new product
        }

        // Handles HTTP GET request to fetch a single product by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var workPackage = await _workpackageRepository.GetByIdAsync<Workpackage>(id); // Calls service to fetch product by ID
                return Ok(workPackage); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkPackages([FromQuery] ComplainPaging complainPaging)
        {
            var workPackages = await _workpackageRepository.GetWorkpackages(complainPaging);
            return Ok(workPackages); // Returns 200 OK response with the list of work packages
        }

        [HttpGet]
        [Route("ticket/{ticketId}")]
        public async Task<IActionResult> GetWorkpackageByTicketId(int ticketId)
        {
            try
            {
                var workPackages = await _workpackageRepository.GetWorkpackagesByTicketId(ticketId); // Calls service to fetch product by ID
                return Ok(workPackages); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpPost]
        [Route("ticket")]
        //[Route("ticket/{workpackageId}/{ticketId}")]
        public async Task<IActionResult> ManageWorkpackateTicketMapping(TicketPackageRequest request)
        {
            try
            {
                if (request.Action == "Add")
                {
                    await _workpackageRepository.AddWorkpackageMappingByTicketId(request.TicketId, request.WorkpackageId); // Calls service to fetch product by ID
                    return Ok(); // Returns 200 OK response if found
                }
                else
                {
                    await _workpackageRepository.DeleteWorkpackageMappingByTicketId(request.TicketId, request.WorkpackageId);
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

                var complainsByPostNo = await _workpackageRepository.GetSummaryLightPostComplintsByPostId(postNo);
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

                var complainsByPostNo = await _workpackageRepository.GetLightPostsByCenterPoint(point.Latitude, point.Longitude);
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
                var complainsByPostNo = await _workpackageRepository.GetDetailLightPostComplintsByPostIdAndName(postNo, name, "");
                return Ok(complainsByPostNo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }


        [HttpGet("lightpostcomplain/{workPackageId}")]
        public async Task<IActionResult> GetLightPostByWorkPackageId(int workPackageId)
        {
            try
            {
                var workpackage = await _workpackageRepository.GetByIdAsync<LightPostComplain>(workPackageId);
                return Ok(workpackage); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet("lightpost/me/{postNo}")]
        public async Task<IActionResult> GetLightPostByWorkPackageId(string postNo)
        {
            
                var myActiveComplains = await _workpackageRepository.GetLightPostActiveWorkpackagesByMe(postNo);
                return Ok(myActiveComplains); // Returns 200 OK response if found
        }

        [HttpGet("lightpost/active")]
        public async Task<IActionResult> GetActiveLightPostMarkers()
        {
            var activeMarkers = await _workpackageRepository.GetActiveLightPostList();
            return Ok(activeMarkers);
        } 

        [HttpGet("lightpost/assigned")]
        public async Task<IActionResult> GetActiveAndAssignedLightPostMarkers()
        {
            var activeMarkers = await _workpackageRepository.GetActiveAndAssignedLightPostList();
            return Ok(activeMarkers);
        }  

        #region General Complain

        [HttpDelete("general/{complainId}")]
        public async Task<IActionResult> DeleteGeneralComplain(int complainId)
        {
            var existingComplain = await _workpackageRepository.GetByIdAsync<GeneralComplain>(complainId);
            if (existingComplain == null)
            {
                return NotFound();
            }
            existingComplain.IsActive = false;
            await _workpackageRepository.UpdateWorkpackageAsync<GeneralComplain>(existingComplain);
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
            generalComplain.Status = request.Status ?? WorkpackageStatus.New; // Default status if not provided


            var addedComplain = await _workpackageRepository.AddWorkpackageAsync<GeneralComplain>(generalComplain); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = addedComplain.WorkpackageId }, addedComplain);
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
            generalComplain.WorkpackageId = complainId; // Ensure the ID is set for the

            var existingComplain = await _workpackageRepository.GetByIdAsync<GeneralComplain>(complainId);
            if (existingComplain == null)
            {
                return NotFound();
            }

            _mapper.Map(generalComplain, existingComplain);

            var updatedComplain = await _workpackageRepository.UpdateWorkpackageAsync<GeneralComplain>(existingComplain);
            return Ok(updatedComplain);
        }

        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralComplainsPaging([FromQuery] GeneralComplainGetPagingRequest complainPaging)
         {
            var generalComplains = await _workpackageRepository.GetGeneralComplain(complainPaging);
            return Ok(generalComplains); // Returns 200 OK response with the list of work packages
        }

        [HttpGet("generalcomplain/{workPackageId}")]
        public async Task<IActionResult> GetGeneralComplainByWorkPackageId(int workPackageId)
        {
            try
            {
                var generalComplain = await _workpackageRepository.GetByIdAsync<GeneralComplain>(workPackageId);
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
                var projectComplains = await _workpackageRepository.GetProjectComplainsByProjectId(projectId);
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
            projectComplain.Status = WorkpackageStatus.New; // Default status if not provided

            var createdProjectComplain = await _workpackageRepository.AddWorkpackageAsync<ProjectComplain>(projectComplain);
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
            projectComplain.WorkpackageId = complainId; // Ensure the ID is set for the

            var existingProjectComplain = await _workpackageRepository.GetByIdAsync<ProjectComplain>(complainId);
            if (existingProjectComplain == null)
            {
                return NotFound();
            }

            _mapper.Map(projectComplain, existingProjectComplain);

            var updatedComplain = await _workpackageRepository.UpdateWorkpackageAsync<ProjectComplain>(existingProjectComplain);
            return Ok(updatedComplain);
        }

        [HttpGet("projectcomplain/{workPackageId}")]
        public async Task<IActionResult> GetProjectComplainByWorkPackageId(int workPackageId)
        {
            try
            {
                var projectComplain = await _workpackageRepository.GetByIdAsync<ProjectComplain>(workPackageId);
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
            var existingComplain = await _workpackageRepository.GetByIdAsync<ProjectComplain>(complainId);
            if (existingComplain == null)
            {
                return NotFound();
            }
            existingComplain.IsActive = false;
            await _workpackageRepository.UpdateWorkpackageAsync<ProjectComplain>(existingComplain);
            return NoContent();
        }

        #endregion


    }
}
