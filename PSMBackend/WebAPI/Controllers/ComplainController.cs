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
                return BadRequest(ApiResponse<Complain>.Failure("Request cannot be null"));
            }

            try
            {
                var complain = _mapper.Map<Complain>(request);
                complain.Status = request.Status ?? ComplainStatus.New; // Default status if not provided
                var updatedComplain = await _complainRepository.AddComplainAsync(complain); // Calls service to add a new product
                return CreatedAtAction(nameof(GetById), new { id = updatedComplain.ComplainId }, 
                    ApiResponse<Complain>.Success(updatedComplain, "Complain created successfully"));
                // Returns 201 Created response with location header pointing to the new product
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error creating complain", ex.Message));
            }
        }


        [HttpGet("Complain/{ComplainId}")]
        public async Task<IActionResult> GetByComplainId(int ComplainId)
        {
            try
            {
                var complain = await _complainRepository.GetByIdAsync<Complain>(ComplainId);
                if (complain == null)
                {
                    return NotFound(ApiResponse<Complain>.Failure("Complain not found"));
                }
                return Ok(ApiResponse<Complain>.Success(complain, "Complain retrieved successfully")); // Returns 200 OK response if found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving complain", ex.Message));
            }
        }

        

        [HttpPost("lightPost")]
        public async Task<IActionResult> AddComplain(LightPostComplainRequest request)
        {
            try
            {
                var complain = _mapper.Map<LightPostComplain>(request);
                complain.Status = request.Status ?? ComplainStatus.New;

                var updatedComplain = await _complainRepository.AddComplainAsync(complain); // Calls service to add a new product
                return CreatedAtAction(nameof(GetById), new { id = updatedComplain.ComplainId }, 
                    ApiResponse<LightPostComplain>.Success(updatedComplain, "Light post complain created successfully"));
                // Returns 201 Created response with location header pointing to the new product
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error creating light post complain", ex.Message));
            }
        }

        // Handles HTTP GET request to fetch a single product by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var complain = await _complainRepository.GetByIdAsync<Complain>(id); // Calls service to fetch product by ID
                if (complain == null)
                {
                    return NotFound(ApiResponse<Complain>.Failure("Complain not found"));
                }
                return Ok(ApiResponse<Complain>.Success(complain, "Complain retrieved successfully")); // Returns 200 OK response if found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving complain", ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetComplains([FromQuery] ComplainPaging complainPaging)
        {
            try
            {
                var complains = await _complainRepository.GetComplains(complainPaging);
                return Ok(ApiResponse<object>.Success(complains, "Complains retrieved successfully")); // Returns 200 OK response with the list of work packages
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving complains", ex.Message));
            }
        }

        [HttpGet]
        [Route("ticket/{ticketId}")]
        public async Task<IActionResult> GetComplainByTicketId(int ticketId)
        {
            try
            {
                var complains = await _complainRepository.GetComplainsByTicketId(ticketId);
                return Ok(ApiResponse<object>.Success(complains, "Complains by ticket retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving complains by ticket", ex.Message));
            }
        }

        [HttpPost]
        [Route("ticket")]
        public async Task<IActionResult> ManageWorkpackateTicketMapping(TicketComplainRequest request)
        {
            try
            {
                if (request.Action == "Add")
                {
                    await _complainRepository.AddComplainMappingByTicketId(request.TicketId, request.ComplainId);
                    return Ok(ApiResponse<string>.Success("", "Complain mapping added successfully"));
                }
                else
                {
                    await _complainRepository.DeleteComplainMappingByTicketId(request.TicketId, request.ComplainId);
                    return Ok(ApiResponse<string>.Success("", "Complain mapping deleted successfully"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error managing complain ticket mapping", ex.Message));
            }
        }

        [HttpGet]
        [Route("lightPost/{postNo}")]
        public async Task<IActionResult> GetSummaryPostComplainsByPostNo(string postNo)
        {
            try
            {
                var complainsByPostNo = await _complainRepository.GetSummaryLightPostComplintsByPostId(postNo);
                return Ok(ApiResponse<object>.Success(complainsByPostNo, "Light post complains summary retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving light post complains summary", ex.Message));
            }
        }

        [HttpGet]
        [Route("lightPost/near")]
        public async Task<IActionResult> GetNearLightPost([FromQuery] LocationPoint point)
        {
            try
            {
                var lightPosts = await _complainRepository.GetLightPostsByCenterPoint(point.Latitude, point.Longitude);
                return Ok(ApiResponse<object>.Success(lightPosts, "Nearby light posts retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving nearby light posts", ex.Message));
            }
        }

        [HttpGet]
        [Route("lightPost/{postNo}/{name}")]
        public async Task<IActionResult> GetDetailPostComplainsByPostNoAndName(string postNo, string name)
        {
            try
            {
                var complainsByPostNo = await _complainRepository.GetDetailLightPostComplintsByPostIdAndName(postNo, name, "");
                return Ok(ApiResponse<object>.Success(complainsByPostNo, "Light post complains detail retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving light post complains detail", ex.Message));
            }
        }


        [HttpGet("lightpostcomplain/{ComplainId}")]
        public async Task<IActionResult> GetLightPostByComplainId(int ComplainId)
        {
            try
            {
                var complain = await _complainRepository.GetByIdAsync<LightPostComplain>(ComplainId);
                if (complain == null)
                {
                    return NotFound(ApiResponse<LightPostComplain>.Failure("Light post complain not found"));
                }
                return Ok(ApiResponse<LightPostComplain>.Success(complain, "Light post complain retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving light post complain", ex.Message));
            }
        }

        [HttpGet("lightpost/me/{postNo}")]
        public async Task<IActionResult> GetLightPostActiveComplainsByMe(string postNo)
        {
            try
            {
                var myActiveComplains = await _complainRepository.GetLightPostActiveComplainsByMe(postNo);
                return Ok(ApiResponse<object>.Success(myActiveComplains, "My active light post complains retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving my active light post complains", ex.Message));
            }
        }

        [HttpGet("lightpost/active")]
        public async Task<IActionResult> GetActiveLightPostMarkers()
        {
            try
            {
                var activeMarkers = await _complainRepository.GetActiveLightPostList();
                return Ok(ApiResponse<object>.Success(activeMarkers, "Active light post markers retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving active light post markers", ex.Message));
            }
        } 
    
        [HttpGet("lightpost/assigned")]
        public async Task<IActionResult> GetActiveAndAssignedLightPostMarkers()
        {
            try
            {
                var activeMarkers = await _complainRepository.GetActiveAndAssignedLightPostList();
                return Ok(ApiResponse<object>.Success(activeMarkers, "Active and assigned light post markers retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving active and assigned light post markers", ex.Message));
            }
        }  

        [HttpPost("lightpost/getpointincircle")]
        public async Task<IActionResult> GetLightPostsInCircle([FromBody] CircleRequest request)
        {
            try
            {
                var activeMarkers = await _complainRepository.GetLightPostListByLocation(request.Latitude, request.Longitude, request.Statuses ?? new int[0]);
                return Ok(ApiResponse<object>.Success(activeMarkers, "Light posts in circle retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving light posts in circle", ex.Message));
            }
        }  

        #region General Complain

        [HttpDelete("general/{complainId}")]
        public async Task<IActionResult> DeleteGeneralComplain(int complainId)
        {
            try
            {
                var existingComplain = await _complainRepository.GetByIdAsync<GeneralComplain>(complainId);
                if (existingComplain == null)
                {
                    return NotFound(ApiResponse<string>.Failure("General complain not found"));
                }
                existingComplain.IsActive = false;
                await _complainRepository.UpdateComplainAsync<GeneralComplain>(existingComplain);
                return Ok(ApiResponse<string>.Success("", "General complain deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error deleting general complain", ex.Message));
            }
        }

        [HttpPost("general")]
        public async Task<IActionResult> AddGeneralComplain(GeneralComplainAddRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(ApiResponse<GeneralComplain>.Failure("Request cannot be null"));
                }
                var generalComplain = _mapper.Map<GeneralComplain>(request);
                generalComplain.Status = request.Status ?? ComplainStatus.New;

                var addedComplain = await _complainRepository.AddComplainAsync<GeneralComplain>(generalComplain);
                return Ok(ApiResponse<GeneralComplain>.Success(addedComplain, "General complain added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error adding general complain", ex.Message));
            }
        }

        [HttpPut("general/{complainId}")]
        public async Task<IActionResult> UpdateGeneralComplain(int complainId, GeneralComplainUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(ApiResponse<GeneralComplain>.Failure("Request cannot be null"));
                }

                var generalComplain = _mapper.Map<GeneralComplain>(request);
                generalComplain.ComplainId = complainId;

                var existingComplain = await _complainRepository.GetByIdAsync<GeneralComplain>(complainId);
                if (existingComplain == null)
                {
                    return NotFound(ApiResponse<GeneralComplain>.Failure("General complain not found"));
                }

                _mapper.Map(generalComplain, existingComplain);

                var updatedComplain = await _complainRepository.UpdateComplainAsync<GeneralComplain>(existingComplain);
                return Ok(ApiResponse<GeneralComplain>.Success(updatedComplain, "General complain updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error updating general complain", ex.Message));
            }
        }

        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralComplainsPaging([FromQuery] GeneralComplainGetPagingRequest complainPaging)
        {
            try
            {
                var generalComplains = await _complainRepository.GetGeneralComplain(complainPaging);
                return Ok(ApiResponse<object>.Success(generalComplains, "General complains retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving general complains", ex.Message));
            }
        }

        [HttpGet("generalcomplain/{ComplainId}")]
        public async Task<IActionResult> GetGeneralComplainByComplainId(int ComplainId)
        {
            try
            {
                var generalComplain = await _complainRepository.GetByIdAsync<GeneralComplain>(ComplainId);
                if (generalComplain == null)
                {
                    return NotFound(ApiResponse<GeneralComplain>.Failure("General complain not found"));
                }
                return Ok(ApiResponse<GeneralComplain>.Success(generalComplain, "General complain retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving general complain", ex.Message));
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
                return Ok(ApiResponse<object>.Success(projectComplains, "Project complains retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving project complains", ex.Message));
            }
        }

        [HttpPost("projectcomplain")]
        public async Task<IActionResult> AddProjectComplain(ProjectComplainPostRequest request)
        {
            try
            {
                var projectComplain = _mapper.Map<ProjectComplain>(request);
                projectComplain.Status = ComplainStatus.New;

                var createdProjectComplain = await _complainRepository.AddComplainAsync<ProjectComplain>(projectComplain);
                return Ok(ApiResponse<ProjectComplain>.Success(createdProjectComplain, "Project complain added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error adding project complain", ex.Message));
            }
        }

        [HttpPut("projectcomplain/{complainId}")]
        public async Task<IActionResult> UpdateProjectComplain(int complainId, ProjectComplainUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(ApiResponse<ProjectComplain>.Failure("Request cannot be null"));
                }

                var projectComplain = _mapper.Map<ProjectComplain>(request);
                projectComplain.ComplainId = complainId;

                var existingProjectComplain = await _complainRepository.GetByIdAsync<ProjectComplain>(complainId);
                if (existingProjectComplain == null)
                {
                    return NotFound(ApiResponse<ProjectComplain>.Failure("Project complain not found"));
                }

                _mapper.Map(projectComplain, existingProjectComplain);

                var updatedComplain = await _complainRepository.UpdateComplainAsync<ProjectComplain>(existingProjectComplain);
                return Ok(ApiResponse<ProjectComplain>.Success(updatedComplain, "Project complain updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error updating project complain", ex.Message));
            }
        }

        [HttpGet("projectcomplain/{ComplainId}")]
        public async Task<IActionResult> GetProjectComplainByComplainId(int ComplainId)
        {
            try
            {
                var projectComplain = await _complainRepository.GetByIdAsync<ProjectComplain>(ComplainId);
                if (projectComplain == null)
                {
                    return NotFound(ApiResponse<ProjectComplain>.Failure("Project complain not found"));
                }
                return Ok(ApiResponse<ProjectComplain>.Success(projectComplain, "Project complain retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving project complain", ex.Message));
            }
        }

        [HttpDelete("projectcomplain/{complainId}")]
        public async Task<IActionResult> DeleteProjectComplain(int complainId)
        {
            try
            {
                var existingComplain = await _complainRepository.GetByIdAsync<ProjectComplain>(complainId);
                if (existingComplain == null)
                {
                    return NotFound(ApiResponse<string>.Failure("Project complain not found"));
                }
                existingComplain.IsActive = false;
                await _complainRepository.UpdateComplainAsync<ProjectComplain>(existingComplain);
                return Ok(ApiResponse<string>.Success("", "Project complain deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error deleting project complain", ex.Message));
            }
        }

        #endregion


    }
}
