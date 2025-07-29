using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Workpackage.ProjectComplain;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
    [AllowAnonymous]
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

            var workpackage = new Workpackage
            {
                Subject = request.Name,
                Detail = request.Detail,
                
                Status = request.Status // Default status if not provided
            };


            var updatedWorkpackage = await _workpackageRepository.AddWorkpackageAsync(workpackage); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedWorkpackage.WorkpackageId }, updatedWorkpackage);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpPost("lightPost")]
        public async Task<IActionResult> AddComplain(LightPostComplintRequest request)
        {

            var workpackage = new LightPostComplain
            {
                Subject = request.Name,
                Detail = request.Detail,

                Status = request.Status.Value, // Default status if not provided
                LightPostNumber = request.LightPostNumber,
                ClientId = request.ClientId
            };


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
                var workPackage = await _workpackageRepository.GetByIdAsync(id); // Calls service to fetch product by ID
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

                var complainsByPostNo = await _workpackageRepository.GetDetailLightPostComplintsByPostIdAndName(postNo, name);
                return Ok(complainsByPostNo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        #region General Complain

        [HttpPost("general")]
        public async Task<IActionResult> AddGeneralComplain(GeneralComplainAddRequest request)
        {

            var workPackage = new GeneralComplain
            {
                Subject = request.Name,
                Detail = request.Detail,
                Status = request.Status.Value, // Default status if not provided
                IsPrivate = request.IsPrivate,
                ClientId = request.ClientId
            };
  

            var updatedWorkpackage = await _workpackageRepository.AddGeneralComplainAsync(workPackage); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedWorkpackage.WorkpackageId }, updatedWorkpackage);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralComplainsPaging([FromQuery] GeneralComplainGetPagingRequest complainPaging)
        {
            var generalComplains = await _workpackageRepository.GetGeneralComplain(complainPaging);
            return Ok(generalComplains); // Returns 200 OK response with the list of work packages
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

            var createdProjectComplain = await _workpackageRepository.AddProjectComplainAsync(projectComplain);
            return CreatedAtAction(nameof(GetProjectComplainsByProjectId), new { projectId = createdProjectComplain.ProjectId }, createdProjectComplain);
        }

        [HttpGet("projectcomplain/{workPackageId}")]
        public async Task<IActionResult> GetProjectComplainByWorkPackageId(int workPackageId)
        {
            try
            {
                var projectComplain = await _workpackageRepository.GetProjectComplainByWorkpackageId(workPackageId);
                return Ok(projectComplain); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        #endregion


    }
}
