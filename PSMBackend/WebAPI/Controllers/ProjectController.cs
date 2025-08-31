using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Expressions;
using PSMModel.Models;
using PSMModel.Enums;
using PSMWebAPI.DTOs.Project;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    // ...removed [AllowAnonymous] to enforce authentication...
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        public ProjectController(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _projectRepository.GetByIdAsync(id); // Calls service to fetch product by ID
                if (ticket == null)
                {
                    return Ok(ApiResponse<Project>.Failure("Project not found"));
                }
                return Ok(ApiResponse<Project>.Success(ticket, "Project retrieved successfully")); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return Ok(ApiResponse<Project>.Failure("Project not found")); // Returns failure response if project does not exist
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProjectPaging paging)
        {
            var response = await _projectRepository.GetPagingAsync(paging); // Calls service to fetch product by ID
            return Ok(ApiResponse<object>.Success(response, "Projects retrieved successfully")); // Returns 200 OK response if found

        }

        //generate action for create project
        [AllowAnonymous]
        [HttpPost]
        [RequestSizeLimit(10_000_000)] // 10MB limit
        [RequestFormLimits(MultipartBodyLengthLimit = 10_000_000)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Add([FromForm] ProjectPostRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            var fileName = string.Empty;

            if (request.SpecificationDocument != null)
            {
                // Ensure uploads directory exists
                var uploadsDir = Path.Combine("uploads", "projects");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // Logic to save the file
                fileName = Guid.NewGuid().ToString() + request.SpecificationDocument.FileName;
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.SpecificationDocument.CopyToAsync(stream);
                }

            }

            Project project = _mapper.Map<Project>(request);

            // Set the file path manually since AutoMapper can't map IFormFile to string
            if (request.SpecificationDocument != null)
            {
                project.SpecificationDocument = fileName;
            }

            var createdProject = await _projectRepository.AddAsync(project);
            return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
             
        }

        //generate action for update project
        [HttpPut("{id}")]
        [RequestSizeLimit(10_000_000)] // 10MB limit
        [RequestFormLimits(MultipartBodyLengthLimit = 10_000_000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] ProjectPostRequest request)
        {
            if (request == null || id <= 0)
            {
                return BadRequest();
            }

            var existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
            {
                return NotFound();
            } 

            var fileName = string.Empty;
            //save file if provided and get file path
            if (request.SpecificationDocument != null)
            {
                // Ensure uploads directory exists
                var uploadsDir = Path.Combine("uploads", "projects");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // Logic to save the file
                fileName = Guid.NewGuid().ToString() + request.SpecificationDocument.FileName;
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.SpecificationDocument.CopyToAsync(stream);
                }

                //need to delete previous document
                if (!string.IsNullOrEmpty(existingProject.SpecificationDocument))
                {
                    var previousFilePath = Path.Combine(uploadsDir, existingProject.SpecificationDocument);
                    if (System.IO.File.Exists(previousFilePath))
                    {
                        System.IO.File.Delete(previousFilePath);
                    }
                }
            }



            _mapper.Map<ProjectPostRequest, Project>(request, existingProject);

            if (request.SpecificationDocument != null)
            {
                existingProject.SpecificationDocument = fileName;
            }

            var updatedProject = await _projectRepository.UpdateAsync(existingProject);
            return Ok(updatedProject);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProjects()
        {
            var response = await _projectRepository.GetAllProjects(); // Calls service to fetch product by ID
            return Ok(response); // Returns 200 OK response if found

        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetProjectByTypeAndStatusAndName(ProjectQuery query)
        {
            var response = await _projectRepository.GetProjectByTypeAndStatusAndName(query.Type, query.Status, query.Name, query.City, query.IsRecent);
            return Ok(response); // Returns 200 OK response if found
        }

        // Test endpoint for file upload
        [HttpPost]
        [Route("test-upload")]
        [RequestSizeLimit(10_000_000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        public IActionResult TestFileUpload([FromForm] IFormFile specificationDocument)
        {
            if (specificationDocument != null)
            {
                return Ok(new { 
                    fileName = specificationDocument.FileName, 
                    size = specificationDocument.Length,
                    contentType = specificationDocument.ContentType
                });
            }
            return BadRequest("No file received");
        }

        [HttpGet]
        [Route("contractor/{contractorId}")]
        public async Task<IActionResult> GetAllProjectsByContractor(string contractorId)
        {
            try
            {
                var response = await _projectRepository.GetAllProjectsByContractorId(contractorId); // Calls service to fetch product by ID
                return Ok(ApiResponse<IEnumerable<Project>>.Success(response)); // Returns 200 OK response if found
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<Project>>.Failure("Error fetching projects: " + ex.Message));
            }
        }

        #region Project Progress

        // Create new project progress
        [HttpPost("projectprogress")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProjectProgress([FromBody] ProjectProgressRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            try
            {
                var projectProgress = _mapper.Map<ProjectProgress>(request);
                var createdProgress = await _projectRepository.AddProjectProgressAsync(projectProgress);
                return CreatedAtAction(nameof(GetProjectProgressById), 
                    new { id = createdProgress.ProjectProgressId }, createdProgress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating project progress: {ex.Message}");
            }
        }

        // Get all project progress records
        [HttpGet("projectprogress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProjectProgress()
        {
            try
            {
                var progressList = await _projectRepository.GetAllProjectProgressAsync();
                return Ok(progressList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving project progress: {ex.Message}");
            }
        }

        // Get project progress by ID
        [HttpGet("projectprogress/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectProgressById(int id)
        {
            try
            {
                var progress = await _projectRepository.GetProjectProgressByIdAsync(id);
                if (progress == null)
                {
                    return NotFound($"Project progress with ID {id} not found");
                }
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving project progress: {ex.Message}");
            }
        }

        // Get project progress by project ID
        [HttpGet("{projectId}/progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectProgressByProjectId(int projectId)
        {
            try
            {
                var progressList = await _projectRepository.GetProjectProgressByProjectIdAsync(projectId);
                return Ok(progressList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving project progress: {ex.Message}");
            }
        }

        // Get latest project progress for a project
        [HttpGet("{projectId}/progress/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLatestProjectProgress(int projectId)
        {
            try
            {
                var progress = await _projectRepository.GetLatestProjectProgressAsync(projectId);
                if (progress == null)
                {
                    return NotFound($"No progress found for project ID {projectId}");
                }
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving latest project progress: {ex.Message}");
            }
        }

        // Get project progress by status
        [HttpGet("projectprogress/status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectProgressByStatus(ProjectProgressApprovedStatus status)
        {
            try
            {
                var progressList = await _projectRepository.GetProjectProgressByStatusAsync(status);
                return Ok(progressList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving project progress by status: {ex.Message}");
            }
        }

        // Update project progress
        [HttpPut("projectprogress/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProjectProgress(int id, [FromBody] ProjectProgressRequest request)
        {
            if (request == null || id <= 0)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var existingProgress = await _projectRepository.GetProjectProgressByIdAsync(id);
                if (existingProgress == null)
                {
                    return NotFound($"Project progress with ID {id} not found");
                }

                _mapper.Map(request, existingProgress);
                var updatedProgress = await _projectRepository.UpdateProjectProgressAsync(existingProgress);
                return Ok(updatedProgress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating project progress: {ex.Message}");
            }
        }

        // Delete project progress
        [HttpDelete("projectprogress/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProjectProgress(int id)
        {
            try
            {
                var result = await _projectRepository.DeleteProjectProgressAsync(id);
                if (!result)
                {
                    return NotFound($"Project progress with ID {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting project progress: {ex.Message}");
            }
        }

        #endregion

    }

    public class ProjectQuery
    {
        public ProjectType? Type { get; set; }
        public ProjectStatus? Status { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public bool IsRecent { get; set; }
    }
}
