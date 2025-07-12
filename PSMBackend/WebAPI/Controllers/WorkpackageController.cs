using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkpackageController : ControllerBase
    {
        private readonly IWorkpackageRepository _workpackageRepository; // Repository instance for database operations
        public WorkpackageController(IWorkpackageRepository workpackageRepository)
        {
            _workpackageRepository = workpackageRepository;
        }
        //Handles HTTP POST request to add a new product
        [HttpPost]
        public async Task<IActionResult> Add(WorkpackageRequest request)
        {

            var workPackage = new WorkPackage
            {
                Name = request.Name,
                Detail = request.Detail,
                CreatedDate = PSMDateTime.Now, // Uses the utility class to get current time in Colombo timezone
                UpdatedDate = PSMDateTime.Now,
                Status = request.Status ?? "New" // Default status if not provided
            };


            var updatedWorkpackage = await _workpackageRepository.AddWorkpackageAsync(workPackage); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedWorkpackage.WorkPackageId }, updatedWorkpackage);
            // Returns 201 Created response with location header pointing to the new product
        }

        [HttpPost("lightPost")]
        public async Task<IActionResult> AddComplain(LightPostComplintRequest request)
        {

            var workPackage = new LightPostComplint
            {
                Name = request.Name,
                Detail = request.Detail,
                CreatedDate = PSMDateTime.Now, // Uses the utility class to get current time in Colombo timezone
                UpdatedDate = PSMDateTime.Now,
                Status = request.Status ?? "New", // Default status if not provided
                LightPostNumber = request.LightPostNumber,
                ClientId = request.ClientId
            };


            var updatedWorkpackage = await _workpackageRepository.AddWorkpackageAsync(workPackage); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedWorkpackage.WorkPackageId }, updatedWorkpackage);
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
            var workPackages = await _workpackageRepository.GetWorkPackages(complainPaging);
            return Ok(workPackages); // Returns 200 OK response with the list of work packages
        }

        [HttpGet]
        [Route("ticket/{ticketId}")]
        public async Task<IActionResult> GetWorkpackageByTicketId(int ticketId)
        {
            try
            {
                var workPackages = await _workpackageRepository.GetWorkPackagesByTicketId(ticketId); // Calls service to fetch product by ID
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
        public async Task<IActionResult> GetNearLightPost([FromQuery] LocationPoint  point)
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
                Name = request.Name,
                Detail = request.Detail,
                CreatedDate = PSMDateTime.Now, // Uses the utility class to get current time in Colombo timezone
                UpdatedDate = PSMDateTime.Now,
                Status = request.Status ?? "New", // Default status if not provided
                IsPrivate = request.IsPrivate,
                ClientId = request.ClientId
            };


            var updatedWorkpackage = await _workpackageRepository.AddGeneralComplainAsync(workPackage); // Calls service to add a new product
            return CreatedAtAction(nameof(GetById), new { id = updatedWorkpackage.WorkPackageId }, updatedWorkpackage);
            // Returns 201 Created response with location header pointing to the new product
        }
        
         [HttpGet("general")]
        public async Task<IActionResult> GetGeneralComplainsPaging([FromQuery] GeneralComplainGetPagingRequest complainPaging)
        {
            var generalComplains = await _workpackageRepository.GetGeneralComplain(complainPaging);
            return Ok(generalComplains); // Returns 200 OK response with the list of work packages
        }

        #endregion


    }
}
